using FluentValidation;
using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.DTOs
{
    public class TraditionalSimulationInputValidator : AbstractValidator<TraditionalSimulationInput>
    {
        public TraditionalSimulationInputValidator()
        {
            RuleFor(x => x.InitialAmount).GreaterThan(0);
            RuleFor(x => x.InterestRate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TimePeriod).GreaterThan(0);
        }
    }
}