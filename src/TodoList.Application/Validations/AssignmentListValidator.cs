using FluentValidation;

namespace TodoList.Application.Validations;

public class AssignmentListValidator : AbstractValidator<Domain.Models.AssignmentList>
{
    public AssignmentListValidator()
    {
        RuleFor(assignmentList => assignmentList.Name)
            .NotNull()
            .WithMessage("Name cannot be null")
            .Length(1, 100)
            .WithMessage("The name must contain between {MinLength} and {MaxLength} characters");
    }
}