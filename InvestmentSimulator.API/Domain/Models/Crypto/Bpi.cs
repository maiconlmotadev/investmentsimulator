using System.Text.Json.Serialization;

namespace InvestmentSimulator.Domain.Models.Crypto
{
    public class Bpi
    {
        [JsonPropertyName("USD")]
        public Usd? Usd { get; set; }
    }
}
