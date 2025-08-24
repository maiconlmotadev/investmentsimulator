namespace InvestmentSimulator.Client.Models
{
    public class TraditionalSimulationInput
    {
        public decimal InitialAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TimePeriod { get; set; }
        public string? UserId { get; set; }
    }
}