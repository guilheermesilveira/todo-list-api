using FluentValidation;

namespace TodoList.Application.Validations.AssignmentList;

public class ValidatorToCreateAndUpdateAssignmentList : AbstractValidator<Domain.Models.AssignmentList>
{
    public ValidatorToCreateAndUpdateAssignmentList()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("O nome não pode ser nulo.")
            .Length(1, 100)
            .WithMessage("O nome deve conter entre {MinLength} e {MaxLength} caracteres.");
    }
}