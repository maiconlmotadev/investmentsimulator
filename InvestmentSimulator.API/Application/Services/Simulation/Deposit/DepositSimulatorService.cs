using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InvestmentSimulator.Application.Services;
using InvestmentSimulator.Domain.Models.Simulation.Deposit;
using InvestmentSimulator.Domain.Models.Ecb;

namespace InvestmentSimulator.Application.Services.Simulation.Deposit
{
    public class DepositSimulatorService : IDepositSimulatorService
    {
        private readonly IEcbDataService _ecbDataService;

        public DepositSimulatorService(IEcbDataService ecbDataService)
        {
            _ecbDataService = ecbDataService;
        }

        public async Task<DepositResult> SimulateAsync(DepositInput input)
        {
            // 1. Get data from ECB
            var interestRateRaw = input.Term == InvestmentTerm.UpTo1Year
                ? await _ecbDataService.GetDepositRateUpToOneYearAsync()
                : await _ecbDataService.GetDepositRateOverTwoYearsAsync();

            var inflationRateRaw = await _ecbDataService.GetLatestInflationRateAsync();

            if (interestRateRaw == null || inflationRateRaw == null)
            {
                // Or handle this more gracefully
                throw new InvalidOperationException("Could not retrieve necessary rates from ECB.");
            }

            var interestRate = (decimal)interestRateRaw.Value / 100;
            var inflationRate = (decimal)inflationRateRaw.Value / 100;
            var taxRate = (input.TaxRate ?? 0) / 100;

            // 2. Simulation Logic
            var result = new DepositResult
            {
                InterestRateUsed = interestRate * 100,
                InflationRateUsed = inflationRate * 100,
                Evolution = new List<DepositYearlyBreakdown>()
            };

            decimal currentGrossValue = input.InitialAmount;
            int years = input.TimePeriodInMonths / 12;

            for (int i = 1; i <= years; i++)
            {
                var yearlyBreakdown = new DepositYearlyBreakdown { Year = i, StartingBalance = currentGrossValue };
                
                decimal interestEarnedThisYear = currentGrossValue * interestRate;
                currentGrossValue += interestEarnedThisYear;

                yearlyBreakdown.InterestEarned = interestEarnedThisYear;
                yearlyBreakdown.EndingBalance = currentGrossValue;
                result.Evolution.Add(yearlyBreakdown);
            }

            // 3. Calculate final results
            result.FinalGrossValue = currentGrossValue;
            result.TotalInterestEarned = result.FinalGrossValue - input.InitialAmount;
            result.TotalTaxPaid = result.TotalInterestEarned * taxRate;
            result.NetInterestEarned = result.TotalInterestEarned - result.TotalTaxPaid;
            result.FinalNetValue = input.InitialAmount + result.NetInterestEarned;

            // 4. Inflation calculation (simplified as total period inflation)
            decimal totalInflationEffect = input.InitialAmount * (decimal)Math.Pow(1 + (double)inflationRate, years) - input.InitialAmount;
            result.TotalValueLostToInflation = totalInflationEffect;
            result.FinalRealValue = result.FinalGrossValue - totalInflationEffect;
            result.RealInterestEarned = result.NetInterestEarned - totalInflationEffect;

            return result;
        }
    }
}
