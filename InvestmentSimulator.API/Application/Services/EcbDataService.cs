using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using InvestmentSimulator.Domain.Models.Ecb;

namespace InvestmentSimulator.Application.Services
{
    public class EcbDataService : IEcbDataService
    {
        private readonly HttpClient _httpClient;
        private const string InflationSeriesKey = "ICP.M.U2.N.000000.4.ANR";
        private const string DepositRateUpToOneYearKey = "MIR.M.U2.B.L22.F.R.A.2250.EUR.N";
        private const string DepositRateOverTwoYearsKey = "MIR.M.U2.B.L22.H.R.A.2250.EUR.N";

        public EcbDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://data-api.ecb.europa.eu/service/data/");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
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

        private async Task<double?> GetLatestValueFromApiAsync(string seriesKey)
        {
            var response = await _httpClient.GetAsync(seriesKey);
            if (!response.IsSuccessStatusCode) return null;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var ecbData = JsonSerializer.Deserialize<EcbDataResponse>(jsonResponse);

            // Navigate through the complex structure to find the latest observation
            var latestObservation = ecbData?.DataSets?.FirstOrDefault()?.Series?.FirstOrDefault().Value?.Observations?.LastOrDefault().Value?.FirstOrDefault();

            return latestObservation;
        }
    }
}
