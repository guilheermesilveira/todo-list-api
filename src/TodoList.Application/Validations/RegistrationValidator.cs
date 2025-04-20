using FluentValidation;

namespace TodoList.Application.Validations;

public class RegistrationValidator : AbstractValidator<Domain.Models.User>
{
    public RegistrationValidator()
    {
        RuleFor(u => u.Name)
            .NotNull()
            .WithMessage("The name cannot be null")
            .Length(3, 50)
            .WithMessage("The name must contain between {MinLength} and {MaxLength} characters");

        RuleFor(u => u.Email)
            .NotNull()
            .WithMessage("Email cannot be null")
            .EmailAddress()
            .WithMessage("The email provided is not valid");

        RuleFor(u => u.Password)
            .NotNull()
            .WithMessage("Password cannot be null")
            .MinimumLength(5)
            .WithMessage("Password must contain at least {MinLength} characters");
    }
}