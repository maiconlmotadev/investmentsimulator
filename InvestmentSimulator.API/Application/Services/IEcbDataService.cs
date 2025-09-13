namespace InvestmentSimulator.Application.Services
{
    public interface IEcbDataService
    {
        Task<double?> GetLatestInflationRateAsync();
        Task<double?> GetDepositRateUpToOneYearAsync();
        Task<double?> GetDepositRateOverTwoYearsAsync();
    }
}
