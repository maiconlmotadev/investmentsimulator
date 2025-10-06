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
            double? interestRateRaw = null;
            double? inflationRateRaw = null;
            bool usedFallbackData = false;

            try
            {
                interestRateRaw = input.Term == InvestmentTerm.UpTo1Year
                    ? await _ecbDataService.GetDepositRateUpToOneYearAsync()
                    : await _ecbDataService.GetDepositRateOverTwoYearsAsync();

                inflationRateRaw = await _ecbDataService.GetLatestInflationRateAsync();
            }
            catch (Exception)
            {
                // Silently catch the exception and proceed with fallback data.
                // The logging inside EcbDataService is sufficient to diagnose the external API issue.
            }

            if (interestRateRaw == null || inflationRateRaw == null)
            {
                interestRateRaw = 2.5; // Fallback interest rate in %
                inflationRateRaw = 2.0;  // Fallback inflation rate in %
                usedFallbackData = true;
            }

            var interestRate = (decimal)interestRateRaw.Value / 100;
            var inflationRate = (decimal)inflationRateRaw.Value / 100;
            var taxRate = (input.TaxRate ?? 0) / 100;

            // 2. Simulation Logic
            var result = new DepositResult
            {
                InterestRateUsed = interestRate * 100,
                InflationRateUsed = inflationRate * 100,
                UsedFallbackData = usedFallbackData,
                Evolution = new List<DepositYearlyBreakdown>()
            };

            // 2.1. Converter taxas anuais para mensais para cálculo de juros compostos
            var monthlyInterestRate = (decimal)Math.Pow(1 + (double)interestRate, 1.0 / 12.0) - 1;
            var monthlyInflationRate = (decimal)Math.Pow(1 + (double)inflationRate, 1.0 / 12.0) - 1;
            
            // Calcula a taxa de juro real mensal
            var monthlyRealRate = (1 + monthlyInterestRate) / (1 + monthlyInflationRate) - 1;

            decimal currentGrossValue = input.InitialAmount;
            decimal accumulatedInterestThisYear = 0;
            decimal yearlyStartingBalance = input.InitialAmount;

            // 2.2. Simular mês a mês para aplicar juros compostos
            for (int month = 1; month <= input.TimePeriodInMonths; month++)
            {
                decimal interestThisMonth = currentGrossValue * monthlyInterestRate;
                currentGrossValue += interestThisMonth;
                accumulatedInterestThisYear += interestThisMonth;

                // 2.3. Agrupar resultados no final de cada ano (ou no último mês)
                if (month % 12 == 0 || month == input.TimePeriodInMonths)
                {
                    var yearlyBreakdown = new DepositYearlyBreakdown
                    {
                        Year = (month + 11) / 12,
                        StartingBalance = yearlyStartingBalance,
                        InterestEarned = accumulatedInterestThisYear,
                        EndingBalance = currentGrossValue
                    };
                    result.Evolution.Add(yearlyBreakdown);

                    // Reset para o próximo ano
                    yearlyStartingBalance = currentGrossValue;
                    accumulatedInterestThisYear = 0;
                }
            }

            // 3. Calculate final results
            result.FinalGrossValue = currentGrossValue;
            result.TotalInterestEarned = result.FinalGrossValue - input.InitialAmount;
            result.TotalTaxPaid = result.TotalInterestEarned * taxRate;
            result.FinalNetValue = result.FinalGrossValue - result.TotalTaxPaid;
            result.NetInterestEarned = result.FinalNetValue - input.InitialAmount;

            // 4. Inflation calculation using the full period
            result.FinalRealValue = input.InitialAmount * (decimal)Math.Pow(1 + (double)monthlyRealRate, input.TimePeriodInMonths);
            result.TotalValueLostToInflation = result.FinalNetValue - result.FinalRealValue;
            result.RealInterestEarned = result.FinalRealValue - input.InitialAmount;

            return result;
        }
    }
}
