using FluentValidation;
using ToDo.Domain.Models;

namespace ToDo.Domain.Validators;

public class AssignmentListValidator : AbstractValidator<AssignmentList>
{
    public AssignmentListValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome da lista de tarefas não pode ser vazio.");
    }
}