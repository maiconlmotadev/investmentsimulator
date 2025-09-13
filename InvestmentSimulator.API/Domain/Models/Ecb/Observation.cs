using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InvestmentSimulator.Domain.Models.Ecb
{
    public class Observation
    {
        [JsonPropertyName("observations")]
        public Dictionary<string, List<double?>>? Observations { get; set; }
    }
}
