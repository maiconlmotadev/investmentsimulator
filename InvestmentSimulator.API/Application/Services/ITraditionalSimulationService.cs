using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public interface ITraditionalSimulationService
    {
        SimulationResult Simulate(Investment investment);
    }
}
