using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public interface ICryptoSimulationService
    {
        Task<CryptoSimulationResult> SimulateCryptoInvestmentAsync(CryptoInvestment investment);
    }
}