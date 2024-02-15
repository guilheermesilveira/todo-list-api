using FluentValidation;

namespace TodoList.Application.Validations.Assignment;

public class ValidatorToCreateAssignment : AbstractValidator<Domain.Models.Assignment>
{
    public ValidatorToCreateAssignment()
    {
        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage("A descrição não pode ser nula.")
            .Length(1, 200)
            .WithMessage("A descrição deve conter entre {MinLength} e {MaxLength} caracteres.");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.Now)
            .WithMessage("O prazo para conclusão deve possuir uma data maior que a atual.")
            .When(x => x.Deadline.HasValue);
    }
}