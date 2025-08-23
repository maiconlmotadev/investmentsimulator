using System.Text.Json.Serialization;

namespace InvestmentSimulator.Domain.Models.Crypto
{
    public class Usd
    {
        [JsonPropertyName("rate_float")]
        public decimal Rate { get; set; }
    }
}
