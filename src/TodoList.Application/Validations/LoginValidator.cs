using FluentValidation;

namespace TodoList.Application.Validations;

public class LoginValidator : AbstractValidator<Domain.Models.User>
{
    public LoginValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty");
    }
}