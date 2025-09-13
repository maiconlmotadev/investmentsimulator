namespace InvestmentSimulator.Client.Models.Simulation.Deposit
{
    public class DepositInput
    {
        public decimal InitialAmount { get; set; }
        public int TimePeriodInMonths { get; set; }
        public InvestmentTerm Term { get; set; }
        public decimal? TaxRate { get; set; } // Optional tax rate in percentage
    }
}

