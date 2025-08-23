using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace InvestmentSimulator.Domain.Models.Crypto
{
    public class CurrentPrice
    {
        [JsonPropertyName("usd")]
        public decimal Usd { get; set; }
    }
}
