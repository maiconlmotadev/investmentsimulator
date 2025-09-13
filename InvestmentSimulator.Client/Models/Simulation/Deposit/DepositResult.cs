using System.Collections.Generic;

namespace InvestmentSimulator.Client.Models.Simulation.Deposit
{
    public class DepositResult
    {
        // Main results
        public decimal FinalGrossValue { get; set; }
        public decimal FinalNetValue { get; set; } // After tax
        public decimal TotalInterestEarned { get; set; }

        // Breakdown
        public decimal TotalTaxPaid { get; set; }
        public decimal NetInterestEarned { get; set; }

        // Real return (inflation adjusted)
        public decimal FinalRealValue { get; set; }
        public decimal TotalValueLostToInflation { get; set; }
        public decimal RealInterestEarned { get; set; }

        // Context
        public decimal InterestRateUsed { get; set; }
        public decimal InflationRateUsed { get; set; }

        // Evolution
        public List<DepositYearlyBreakdown> Evolution { get; set; } = new List<DepositYearlyBreakdown>();
    }

    public class DepositYearlyBreakdown
    {
        public int Year { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal InterestEarned { get; set; }
        public decimal EndingBalance { get; set; }
    }
}