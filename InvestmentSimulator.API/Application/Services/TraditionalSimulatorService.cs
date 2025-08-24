using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public class TraditionalSimulatorService : ITraditionalSimulatorService
    {
        public TraditionalSimulationResult Simulate(TraditionalSimulationInput investment)
        {
            var result = new TraditionalSimulationResult();
            decimal currentValue = investment.InitialAmount;

            result.Evolution.Add(currentValue);

            for (int i = 0; i < investment.TimePeriod; i++)
            {
                currentValue += currentValue * (investment.InterestRate / 100 / 12); // Juros compostos mensais
                result.Evolution.Add(currentValue);
            }

            result.FinalValue = currentValue;
            return result;
        }
    }
}