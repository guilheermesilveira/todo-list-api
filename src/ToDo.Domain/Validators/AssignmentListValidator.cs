﻿using FluentValidation;
using ToDo.Domain.Models;

namespace ToDo.Domain.Validators;

public class AssignmentListValidator : AbstractValidator<AssignmentList>
{
    public AssignmentListValidator()
    {
        RuleFor(x => x.Name)
            .Length(1, 100)
            .WithMessage("O nome da lista de tarefas deve conter entre {MinLength} e {MaxLength} caracteres.");
    }
}