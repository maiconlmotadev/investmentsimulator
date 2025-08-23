using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public class TraditionalSimulationService : ITraditionalSimulationService
    {
        public SimulationResult Simulate(Investment investment)
        {
            var result = new SimulationResult();
            decimal currentValue = investment.InitialAmount;

            for (int i = 0; i <= investment.TimePeriod; i++)
            {
                result.Evolution.Add(currentValue);
                currentValue += currentValue * (investment.InterestRate / 100 / 12); // Juros compostos mensais
            }

            result.FinalValue = currentValue;
            return result;
        }
    }
}
