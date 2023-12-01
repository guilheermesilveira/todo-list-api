using FluentValidation;
using ToDo.Domain.Models;

namespace ToDo.Domain.Validators;

public class AssignmentValidator : AbstractValidator<Assignment>
{
    public AssignmentValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("A descrição não pode ser vazia.")
            .Length(3, 200)
            .WithMessage("A descrição deve conter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.Now)
            .WithMessage("O prazo final deve possuir uma data maior que a atual.");
    }
}