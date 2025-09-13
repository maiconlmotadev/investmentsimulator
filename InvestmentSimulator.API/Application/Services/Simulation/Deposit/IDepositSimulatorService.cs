using System.Threading.Tasks;
using InvestmentSimulator.Domain.Models.Simulation.Deposit;

namespace InvestmentSimulator.Application.Services.Simulation.Deposit
{
    public interface IDepositSimulatorService
    {
        Task<DepositResult> SimulateAsync(DepositInput investment);
    }
}
