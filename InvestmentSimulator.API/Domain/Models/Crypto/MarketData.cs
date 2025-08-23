using System.Text.Json.Serialization;

namespace InvestmentSimulator.Domain.Models.Crypto
{
    public class MarketData
    {
        [JsonPropertyName("current_price")]
        public Dictionary<string, decimal>? CurrentPrice { get; set; }
    }
}
