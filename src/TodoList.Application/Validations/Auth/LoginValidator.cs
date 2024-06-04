using FluentValidation;

namespace TodoList.Application.Validations.Auth;

public class LoginValidator : AbstractValidator<Domain.Models.User>
{
    public LoginValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("O email não pode ser vazio.");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("A senha não pode ser vazia.");
    }
}