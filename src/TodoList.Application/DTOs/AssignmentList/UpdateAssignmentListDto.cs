using FluentValidation;
using FluentValidation.Results;

namespace TodoList.Application.DTOs.AssignmentList;

public class UpdateAssignmentListDto
{
    public string Name { get; set; } = null!;

    public bool Validate(out ValidationResult validationResult)
    {
        var validator = new InlineValidator<UpdateAssignmentListDto>();

        validator
            .RuleFor(x => x.Name)
            .Length(1, 100)
            .WithMessage("O nome da lista de tarefas deve conter entre {MinLength} e {MaxLength} caracteres.");

        validationResult = validator.Validate(this);
        return validationResult.IsValid;
    }
}