using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InvestmentSimulator.Domain.Models.Ecb
{
    public class EcbDataResponse
    {
        [JsonPropertyName("dataSets")]
        public List<DataSet> DataSets { get; set; }
    }

    public class DataSet
    {
        [JsonPropertyName("series")]
        public Dictionary<string, SeriesDetail> Series { get; set; }
    }

    public class SeriesDetail
    {
        [JsonPropertyName("observations")]
        public Dictionary<string, List<double?>> Observations { get; set; }
    }
}