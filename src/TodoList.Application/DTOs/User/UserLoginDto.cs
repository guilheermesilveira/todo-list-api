using FluentValidation;
using FluentValidation.Results;

namespace TodoList.Application.DTOs.User;

public class UserLoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public bool Validate(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<UserLoginDto>();

        validator
            .RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("O email deve ser informado.");

        validator
            .RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("A senha deve ser informada.");

        validationResult = validator.Validate(this);
        return validationResult.IsValid;
    }
}