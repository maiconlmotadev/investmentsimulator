using System.Text.Json.Serialization;

namespace InvestmentSimulator.Domain.Models.Crypto
{
    public class CryptoPrice
    {
        [JsonPropertyName("bpi")]
        public Bpi? Bpi { get; set; }
    }
}
