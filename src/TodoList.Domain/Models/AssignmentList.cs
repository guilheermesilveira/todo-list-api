using System.Collections.ObjectModel;

namespace TodoList.Domain.Models;

public class AssignmentList : Entity
{
    public string Name { get; set; } = null!;
    public int UserId { get; set; }

    // EF Relation
    public virtual User User { get; set; } = null!;
    public virtual Collection<Assignment> Assignments { get; set; } = new();
}