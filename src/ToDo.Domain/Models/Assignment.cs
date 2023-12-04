using FluentValidation.Results;
using ToDo.Domain.Validators;

namespace ToDo.Domain.Models;

public class Assignment : Entity
{
    public int UserId { get; set; }
    public int AssignmentListId { get; set; }
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public bool Concluded { get; private set; }
    public DateTime? ConcludedAt { get; private set; }
    public User User { get; set; } = null!;
    public AssignmentList AssignmentList { get; set; } = null!;
    
    public void SetConcluded()
    {
        Concluded = true;
        ConcludedAt = DateTime.Now;
    }

    public void SetUnconcluded()
    {
        Concluded = false;
        ConcludedAt = null;
    }
    
    public override bool Validate(out ValidationResult validationResult)
    {
        validationResult = new AssignmentValidator().Validate(this);
        return validationResult.IsValid;
    }
}