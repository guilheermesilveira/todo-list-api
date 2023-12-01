using System.Collections.ObjectModel;
using FluentValidation.Results;
using ToDo.Domain.Validators;

namespace ToDo.Domain.Models;

public class AssignmentList : Entity
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public User User { get; set; } = null!;
    public Collection<Assignment> Assignments { get; set; } = new();
    
    public override bool Validate(out ValidationResult validationResult)
    {
        validationResult = new AssignmentListValidator().Validate(this);
        return validationResult.IsValid;
    }
}