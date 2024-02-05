using FluentValidation;
using FluentValidation.Results;

namespace TodoList.Application.DTOs.Assignment;

public class UpdateAssignmentDto
{
    public string? Description { get; set; }
    public DateTime Deadline { get; set; }
    public int AssignmentListId { get; set; }

    public bool Validate(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<UpdateAssignmentDto>();

        if (!string.IsNullOrEmpty(Description))
        {
            validator
                .RuleFor(x => x.Description)
                .Length(1, 200)
                .WithMessage("A descrição da tarefa deve conter entre {MinLength} e {MaxLength} caracteres.");
        }

        validator
            .RuleFor(x => x.Deadline)
            .GreaterThan(DateTime.Now)
            .WithMessage("O prazo final da tarefa deve possuir uma data maior que a atual.");

        validationResult = validator.Validate(this);
        return validationResult.IsValid;
    }
}