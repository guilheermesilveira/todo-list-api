using FluentValidation;

namespace TodoList.Application.Validations.Assignment;

public class ValidatorToUpdateAssignment : AbstractValidator<Domain.Models.Assignment>
{
    public ValidatorToUpdateAssignment()
    {
        RuleFor(assignment => assignment.Description)
            .Length(1, 200)
            .WithMessage("A descrição deve conter entre {MinLength} e {MaxLength} caracteres.")
            .When(assignment => !string.IsNullOrEmpty(assignment.Description));

        RuleFor(assignment => assignment.Deadline)
            .GreaterThan(DateTime.Now)
            .WithMessage("O prazo para conclusão deve possuir uma data maior que a atual.")
            .When(assignment => assignment.Deadline.HasValue);
    }
}