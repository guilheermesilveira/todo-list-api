using System.Collections.ObjectModel;

namespace ToDo.Domain.Models;

public class User : Entity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Collection<Assignment> Assignments { get; set; } = new();
    public Collection<AssignmentList> AssignmentLists { get; set; } = new();
}