using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public interface ISimulationService
    {
        SimulationResult Simulate(Investment investment);
    }
}