using FluentValidation;

namespace TodoList.Application.Validations;

public class AssignmentValidator : AbstractValidator<Domain.Models.Assignment>
{
    public AssignmentValidator()
    {
        RuleFor(assignment => assignment.Description)
            .NotNull()
            .WithMessage("Description cannot be null")
            .Length(1, 200)
            .WithMessage("The description must contain between {MinLength} and {MaxLength} characters");

        RuleFor(assignment => assignment.Deadline)
            .GreaterThan(DateTime.Now)
            .WithMessage("The deadline for completion must be a date later than the current one")
            .When(assignment => assignment.Deadline.HasValue);

        RuleFor(assignment => assignment.AssignmentListId)
            .GreaterThan(0)
            .WithMessage("The assignment list id must be provided");
    }
}