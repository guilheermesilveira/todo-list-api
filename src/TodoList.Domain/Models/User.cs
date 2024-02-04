using System.Collections.ObjectModel;

namespace TodoList.Domain.Models;

public class User : Entity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    // EF Relation
    public virtual Collection<AssignmentList> AssignmentLists { get; set; } = new();
    public virtual Collection<Assignment> Assignments { get; set; } = new();
}