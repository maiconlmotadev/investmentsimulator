using System.Text.Json.Serialization;

namespace InvestmentSimulator.Domain.Models.Crypto
{
    public class CoinGeckoHistoricalPriceResponse
    {
        [JsonPropertyName("market_data")]
        public MarketData? MarketData { get; set; }
    }
}
