using FluentValidation;

namespace TodoList.Application.Validations.User;

public class ValidatorToRegisterUser : AbstractValidator<Domain.Models.User>
{
    public ValidatorToRegisterUser()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("O nome não pode ser nulo.")
            .Length(3, 50)
            .WithMessage("O nome deve conter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("O email não pode ser nulo.")
            .EmailAddress()
            .WithMessage("O email fornecido não é válido.")
            .MaximumLength(100)
            .WithMessage("O email deve conter no máximo {MaxLength} caracteres.");

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("A senha não pode ser nula.")
            .MinimumLength(5)
            .WithMessage("A senha deve conter no mínimo {MinLength} caracteres.");
    }
}