using FluentValidation;
using InvestmentSimulator.Domain.Models.Simulation.Deposit;

namespace InvestmentSimulator.Application.DTOs.Simulation.Deposit
{
    public class DepositInputValidator : AbstractValidator<DepositInput>
    {
        public DepositInputValidator()
        {
            RuleFor(x => x.InitialAmount).GreaterThan(0);
            RuleFor(x => x.TimePeriodInMonths).GreaterThan(0);
            RuleFor(x => x.Term).IsInEnum();
            RuleFor(x => x.TaxRate).InclusiveBetween(0, 100).When(x => x.TaxRate.HasValue);
        }
    }
}
