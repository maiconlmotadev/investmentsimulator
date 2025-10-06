using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using InvestmentSimulator.Domain.Models.Ecb;
using Microsoft.Extensions.Caching.Memory;
using System;
using Microsoft.Extensions.Logging;

namespace InvestmentSimulator.Application.Services
{
    public class EcbDataService : IEcbDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<EcbDataService> _logger;
        private const string InflationSeriesKey = "ICP.M.U2.N.000000.4.ANR";
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
            return await GetLatestValueFromApiAsync(InflationSeriesKey);
        }

        public async Task<double?> GetDepositRateUpToOneYearAsync()
        {
            return await GetLatestValueFromApiAsync(DepositRateUpToOneYearKey);
        }

        public async Task<double?> GetDepositRateOverTwoYearsAsync()
        {
            return await GetLatestValueFromApiAsync(DepositRateOverTwoYearsKey);
        }

        public async Task<double?> GetDepositFacilityRateAsync()
        {
            return await GetLatestValueFromApiAsync(DepositFacilityRateKey);
        }

        public async Task<double?> GetTenYearGovernmentBondYieldAsync()
        {
            return await GetLatestValueFromApiAsync(TenYearGovernmentBondYieldKey);
        }

        private async Task<double?> GetLatestValueFromApiAsync(string seriesKey)
        {
            // Tenta obter o valor do cache primeiro. A chave do cache é a própria seriesKey.
            if (_cache.TryGetValue(seriesKey, out double? cachedValue))
            {
                return cachedValue;
            }

            // Se não estiver no cache, busca na API.
            var valueFromApi = await FetchValueFromApi(seriesKey);

            if (valueFromApi.HasValue)
            {
                // Armazena o valor obtido no cache com uma validade de 12 horas.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(12));

                _cache.Set(seriesKey, valueFromApi, cacheEntryOptions);
            }

            return valueFromApi;
        }

        private async Task<double?> FetchValueFromApi(string seriesKey)
        {
            try
            {
                var response = await _httpClient.GetAsync(seriesKey);
                _logger.LogInformation("ECB API request for series {SeriesKey}: StatusCode={StatusCode}", seriesKey, response.StatusCode);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("ECB API response for series {SeriesKey}: {JsonResponse}", seriesKey, jsonResponse);

                // Lança uma exceção se a resposta da API não for bem-sucedida (ex: 404, 500).
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("ECB API returned a non-success status code for series {SeriesKey}: {StatusCode}. Response: {JsonResponse}", seriesKey, response.StatusCode, jsonResponse);
                    throw new HttpRequestException($"ECB API request for series '{seriesKey}' failed with status code {response.StatusCode}.");
                }

                // Adiciona PropertyNameCaseInsensitive para maior robustez na desserialização.
                var ecbData = JsonSerializer.Deserialize<EcbDataResponse>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (!TryGetLatestObservation(ecbData, out var latestObservation))
                {
                    // Lança um erro se a estrutura da resposta for válida, mas o dado não for encontrado.
                    throw new InvalidOperationException($"Não foi possível encontrar a última observação para a série '{seriesKey}' na resposta da API do BCE.");
                }
                return latestObservation;
            }
            catch (HttpRequestException ex)
            {
                // Captura erros de rede ou códigos de status de erro HTTP.
                _logger.LogError(ex, "Erro ao comunicar com a API do BCE para a série '{SeriesKey}'.", seriesKey);
                throw new InvalidOperationException($"Erro ao comunicar com a API do BCE para a série '{seriesKey}'.", ex);
            }
        }

        private bool TryGetLatestObservation(EcbDataResponse? ecbData, out double? observationValue)
        {
            observationValue = null;
            if (ecbData?.DataSets == null || !ecbData.DataSets.Any())
            {
                _logger.LogWarning("ECB response for does not contain DataSets.");
                return false;
            }

            var series = ecbData.DataSets.First().Series;
            if (series == null || !series.Any())
            {
                _logger.LogWarning("ECB DataSet does not contain Series.");
                return false;
            }

            var observations = series.First().Value.Observations;
            if (observations == null || !observations.Any())
            {
                _logger.LogWarning("ECB Series does not contain Observations.");
                return false;
            }

            var lastObservation = observations.Last().Value;
            if (lastObservation == null || !lastObservation.Any())
            {
                _logger.LogWarning("ECB last observation does not contain a value list.");
                return false;
            }

            observationValue = lastObservation.First();
            if (!observationValue.HasValue)
            {
                _logger.LogWarning("ECB last observation value is null.");
                return false;
            }
            
            return true;
        }
    }
}