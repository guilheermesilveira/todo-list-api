using FluentValidation;

namespace TodoList.Application.Validations.Assignment;

public class ValidatorToCreateAssignment : AbstractValidator<Domain.Models.Assignment>
{
    public ValidatorToCreateAssignment()
    {
        RuleFor(assignment => assignment.Description)
            .NotNull()
            .WithMessage("A descrição não pode ser nula.")
            .Length(1, 200)
            .WithMessage("A descrição deve conter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(assignment => assignment.Deadline)
            .GreaterThan(DateTime.Now)
            .WithMessage("O prazo para conclusão deve possuir uma data maior que a atual.")
            .When(assignment => assignment.Deadline.HasValue);
    }
}