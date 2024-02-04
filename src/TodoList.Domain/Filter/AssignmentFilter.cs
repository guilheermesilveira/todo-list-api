namespace TodoList.Domain.Filter;

public class AssignmentFilter
{
    public string? Description { get; set; }
    public DateTime? StartDeadline { get; set; }
    public DateTime? EndDeadline { get; set; }
    public bool? Concluded { get; set; }
}