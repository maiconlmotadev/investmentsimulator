using System.Collections.Generic;

namespace InvestmentSimulator.Domain.Models.Ecb
{
    // The structure of the "series" object is dynamic, e.g., "0:0:0:0:0:0:0".
    // We use a Dictionary to capture this, where the key is the series identifier
    // and the value is the Observation object.
    public class Series : Dictionary<string, Observation>
    {
    }
}
