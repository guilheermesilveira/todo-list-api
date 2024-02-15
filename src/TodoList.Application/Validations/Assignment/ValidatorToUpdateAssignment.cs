using FluentValidation;

namespace TodoList.Application.Validations.Assignment;

public class ValidatorToUpdateAssignment : AbstractValidator<Domain.Models.Assignment>
{
    public ValidatorToUpdateAssignment()
    {
        RuleFor(x => x.Description)
            .Length(1, 200)
            .WithMessage("A descrição deve conter entre {MinLength} e {MaxLength} caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.Now)
            .WithMessage("O prazo para conclusão deve possuir uma data maior que a atual.")
            .When(x => x.Deadline.HasValue);
    }
}