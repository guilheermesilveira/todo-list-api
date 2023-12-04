using FluentValidation;
using FluentValidation.Results;

namespace ToDo.Application.DTOs.User;

public class UserLoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public bool Validate(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<UserLoginDto>();
        
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

        validationResult = validator.Validate(this);
        
        return validationResult.IsValid;
    }
}