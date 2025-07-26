namespace InvestmentSimulator.Domain.Models
{
    public class SimulationResult
    {
        public decimal FinalValue { get; set; }
        public List<decimal> Evolution { get; set; } = new List<decimal>();
    }
}