namespace TodoList.Application.DTOs.Assignment;

public class SearchAssignmentDto
{
    public string? Description { get; set; }
    public DateTime? StartDeadline { get; set; }
    public DateTime? EndDeadline { get; set; }
    public bool? Concluded { get; set; }
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
    public int? AssignmentListId { get; set; }
}