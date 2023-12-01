namespace ToDo.Domain.Filter;

public class AssignmentFilter
{ 
    public string Description { get; set; } = null!;
    public DateTime? StartDeadline { get; set; }
    public DateTime? EndDeadline { get; set; }
    public bool? Concluded { get; set; }
}