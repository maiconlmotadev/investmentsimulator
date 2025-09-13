using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InvestmentSimulator.Domain.Models.Ecb
{
    public class EcbDataResponse
    {
        [JsonPropertyName("dataSets")]
        public List<DataSet>? DataSets { get; set; }
    }
}
