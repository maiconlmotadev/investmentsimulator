using System.Text.Json.Serialization;

namespace InvestmentSimulator.Domain.Models.Ecb
{
    public class DataSet
    {
        [JsonPropertyName("series")]
        public Series? Series { get; set; }
    }
}
