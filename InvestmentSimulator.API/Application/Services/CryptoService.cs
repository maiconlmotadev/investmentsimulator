using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using InvestmentSimulator.Domain.Models.Crypto;

namespace InvestmentSimulator.Application.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly HttpClient _httpClient;

        public CryptoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetCurrentPriceAsync(string cryptoId)
        {
            var response = await _httpClient.GetAsync($"https://api.coingecko.com/api/v3/simple/price?ids={cryptoId}&vs_currencies=usd");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(responseString);
            if (data == null || !data.ContainsKey(cryptoId) || !data[cryptoId].ContainsKey("usd"))
            {
                throw new Exception($"Could not get current price for {cryptoId}");
            }
            return data[cryptoId]["usd"];
        }

        public async Task<decimal> GetHistoricalPriceAsync(string cryptoId, DateTime date)
        {
            var dateString = date.ToString("dd-MM-yyyy");
            var response = await _httpClient.GetAsync($"https://api.coingecko.com/api/v3/coins/{cryptoId}/history?date={dateString}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<CoinGeckoHistoricalPriceResponse>(responseString);

            if (data == null || data.MarketData?.CurrentPrice == null || !data.MarketData.CurrentPrice.ContainsKey("usd"))
            {
                throw new Exception($"Could not get historical price for {cryptoId} on {dateString}");
            }
            return data.MarketData.CurrentPrice["usd"];
        }
    }
}
