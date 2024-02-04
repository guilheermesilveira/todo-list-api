using FluentValidation;
using FluentValidation.Results;

namespace TodoList.Application.DTOs.User;

public class CreateUserDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;

    public bool Validate(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<CreateUserDto>();

        validator
            .RuleFor(x => x.Name)
            .Length(3, 50)
            .WithMessage("O nome deve conter entre {MinLength} e {MaxLength} caracteres.");

        validator
            .RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("O email fornecido não é válido.")
            .MaximumLength(100)
            .WithMessage("O email deve conter no máximo {MaxLength} caracteres.");

        validator
            .RuleFor(x => x.Password)
            .MinimumLength(5)
            .WithMessage("A senha deve conter no mínimo {MinLength} caracteres.");

        validator
            .RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("As senhas não coincidem.");

        validationResult = validator.Validate(this);
        return validationResult.IsValid;
    }
}