using FluentValidation;

namespace InvestmentSimulator.Application.DTOs
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}