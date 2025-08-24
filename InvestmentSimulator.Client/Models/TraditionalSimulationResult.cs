using System.Collections.Generic;

namespace InvestmentSimulator.Client.Models
{
    public class TraditionalSimulationResult
    {
        public decimal FinalValue { get; set; }
        public List<decimal> Evolution { get; set; } = new List<decimal>();
    }
}