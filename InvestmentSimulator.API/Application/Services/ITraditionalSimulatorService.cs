using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public interface ITraditionalSimulatorService
    {
        TraditionalSimulationResult Simulate(TraditionalSimulationInput investment);
    }
}