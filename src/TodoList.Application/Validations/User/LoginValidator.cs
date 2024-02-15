using FluentValidation;

namespace TodoList.Application.Validations.User;

public class LoginValidator : AbstractValidator<Domain.Models.User>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("O email não pode ser vazio.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("A senha não pode ser vazia.");
    }
}