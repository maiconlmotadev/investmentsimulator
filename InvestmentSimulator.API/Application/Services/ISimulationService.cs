namespace InvestmentSimulator.Application.Services
{
    public interface ISimulationService
    {
        Task<SimulationResult> SimulateAsync(Investment investment);
    }
}