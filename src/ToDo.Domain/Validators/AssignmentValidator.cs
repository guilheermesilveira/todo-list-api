using FluentValidation;
using ToDo.Domain.Models;

namespace ToDo.Domain.Validators;

public class AssignmentValidator : AbstractValidator<Assignment>
{
    public AssignmentValidator()
    {
        RuleFor(x => x.Description)
            .Length(1, 200)
            .WithMessage("A descrição da tarefa deve conter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.Now)
            .WithMessage("O prazo final da tarefa deve possuir uma data maior que a atual.");
    }
}