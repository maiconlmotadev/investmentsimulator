using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public class SimulationService : ISimulationService
    {
        public async Task<SimulationResult> SimulateAsync(Investment investment)
        {
            var result = new SimulationResult();
            decimal currentValue = investment.InitialAmount;

            for (int i = 0; i <= investment.TimePeriod; i++)
            {
                result.Evolution.Add(currentValue);
                currentValue += currentValue * (investment.InterestRate / 100 / 12); // Juros compostos mensais
                await Task.Delay(1); // Simula assincronia (remova em produção)
            }

            result.FinalValue = currentValue;
            return result;
        }
    }
}