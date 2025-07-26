using FluentValidation;
using InvestmentSimulator.Domain.Models;

namespace InvestmentSimulator.Application.DTOs
{
    public class InvestmentValidator : AbstractValidator<Investment>
    {
        public InvestmentValidator()
        {
            RuleFor(x => x.InitialAmount).GreaterThan(0);
            RuleFor(x => x.InterestRate).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TimePeriod).GreaterThan(0);
        }
    }
}