using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.Services
{
    public class CryptoSimulatorService : ICryptoSimulatorService
    {
        private readonly ICryptoService _cryptoService;

        public CryptoSimulatorService(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
        }

        public async Task<CryptoSimulationResult> SimulateCryptoInvestmentAsync(CryptoSimulationInput investment)
        {
            if (investment.CryptoId == null) throw new ArgumentNullException(nameof(investment.CryptoId));

            var historicalPrice = await _cryptoService.GetHistoricalPriceAsync(investment.CryptoId, investment.PurchaseDate);
            var currentPrice = await _cryptoService.GetCurrentPriceAsync(investment.CryptoId);

            var finalValue = (investment.InitialAmount / historicalPrice) * currentPrice;
            var profit = finalValue - investment.InitialAmount;
            var profitPercentage = (profit / investment.InitialAmount) * 100;

            return new CryptoSimulationResult
            {
                FinalValue = finalValue,
                Profit = profit,
                ProfitPercentage = profitPercentage
            };
        }
    }
}
