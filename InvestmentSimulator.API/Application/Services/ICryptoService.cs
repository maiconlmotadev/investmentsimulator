using InvestmentSimulator.Domain.Models.Crypto;

namespace InvestmentSimulator.Application.Services
{
    public interface ICryptoService
    {
        Task<decimal> GetCurrentPriceAsync(string cryptoId);
        Task<decimal> GetHistoricalPriceAsync(string cryptoId, DateTime date);
    }
}
