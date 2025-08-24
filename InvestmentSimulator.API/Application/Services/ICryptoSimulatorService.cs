using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public interface ICryptoSimulatorService
    {
        Task<CryptoSimulationResult> SimulateCryptoInvestmentAsync(CryptoSimulationInput investment);
    }
}
