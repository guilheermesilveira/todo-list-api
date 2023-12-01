using FluentValidation.Results;
using ToDo.Domain.Contracts.Interfaces;

namespace ToDo.Domain.Models;

public abstract class Entity : IEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual bool Validate(out ValidationResult validationResult)
    {
        validationResult = new ValidationResult();
        return validationResult.IsValid;
    }
}