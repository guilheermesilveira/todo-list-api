namespace ToDo.Application.DTOs.Assignment;

public class SearchAssignmentDto
{
    public int? AssignmentListId { get; set; }
    public string Description { get; set; } = null!;
    public DateTime? StartDeadline { get; set; }
    public DateTime? EndDeadline { get; set; }
    public bool? Concluded { get; set; }
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}