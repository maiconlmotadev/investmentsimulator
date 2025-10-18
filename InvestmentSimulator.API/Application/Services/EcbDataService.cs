
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace InvestmentSimulator.Application.Services
{
    public class EcbDataService : IEcbDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<EcbDataService> _logger;
        private const string InflationSeriesKey = "M.U2.N.000000.4.ANR";
        private const string DepositRateUpToOneYearKey = "MIR.M.U2.B.L22.F.R.A.2250.EUR.N";
        private const string DepositRateOverTwoYearsKey = "MIR.M.U2.B.L22.H.R.A.2250.EUR.N";
        private const string DepositFacilityRateKey = "FM.B.U2.EUR.4F.KR.DFR.HLN";
        private const string TenYearGovernmentBondYieldKey = "YC.B.U2.EUR.4F.G_N_A.SV_C_YM.SR_10Y";

        public EcbDataService(HttpClient httpClient, IMemoryCache cache, ILogger<EcbDataService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("https://data-api.ecb.europa.eu/service/data/");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "InvestmentSimulator/1.0");
        }

        public async Task<double?> GetLatestInflationRateAsync()
        {
            return await GetLatestValueFromApiAsync("ICP", InflationSeriesKey);
        }

        public async Task<double?> GetDepositRateUpToOneYearAsync()
        {
            return await GetLatestValueFromApiAsync("MIR", DepositRateUpToOneYearKey);
        }

        public async Task<double?> GetDepositRateOverTwoYearsAsync()
        {
            return await GetLatestValueFromApiAsync("MIR", DepositRateOverTwoYearsKey);
        }

        public async Task<double?> GetDepositFacilityRateAsync()
        {
            return await GetLatestValueFromApiAsync("FM", DepositFacilityRateKey);
        }

        public async Task<double?> GetTenYearGovernmentBondYieldAsync()
        {
            return await GetLatestValueFromApiAsync("YC", TenYearGovernmentBondYieldKey);
        }

        private async Task<double?> GetLatestValueFromApiAsync(string flowRef, string seriesKey)
        {
            if (_cache.TryGetValue(seriesKey, out double? cachedValue))
            {
                return cachedValue;
            }

            var valueFromApi = await FetchValueFromApi(flowRef, seriesKey);

            if (valueFromApi.HasValue)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12));
                _cache.Set(seriesKey, valueFromApi, cacheEntryOptions);
            }

            return valueFromApi;
        }

        private async Task<double?> FetchValueFromApi(string flowRef, string seriesKey)
        {
            try
            {
                var requestUrl = $"{flowRef}/{seriesKey}?lastnobservations=1&format=jsondata";
                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("ECB API request for series {SeriesKey} failed with status code {StatusCode}.", seriesKey, response.StatusCode);
                    return null; // Retorna nulo em vez de lançar exceção para não quebrar a simulação
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                
                // Usa JsonNode para uma análise robusta e à prova de falhas.
                JsonNode? root = JsonNode.Parse(jsonResponse);

                // Navega pela estrutura do JSON de forma segura.
                JsonNode? observationsNode = root?["dataSets"]?[0]?["series"]?.AsObject().FirstOrDefault().Value?["observations"];

                if (observationsNode == null)
                {
                    _logger.LogWarning("Could not find 'observations' node in ECB response for series {SeriesKey}.", seriesKey);
                    return null;
                }

                // A última observação é o último par chave-valor no objeto de observações.
                var lastObservation = observationsNode.AsObject().LastOrDefault();

                // O valor está no primeiro item do array da observação.
                double? latestValue = lastObservation.Value?[0]?.GetValue<double?>();

                if (latestValue.HasValue)
                {
                    _logger.LogInformation("Successfully extracted value {Value} for series {SeriesKey}.", latestValue.Value, seriesKey);
                    return latestValue;
                }
                else
                {
                    _logger.LogWarning("Latest observation value for series {SeriesKey} is null or not found.", seriesKey);
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Captura qualquer exceção durante o processo para evitar o erro 500.
                _logger.LogError(ex, "An unexpected error occurred in FetchValueFromApi for series {SeriesKey}.", seriesKey);
                return null; // Retorna nulo para que a aplicação continue funcionando com dados de fallback.
            }
        }
    }
}
