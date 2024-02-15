namespace TodoList.Application.DTOs.Assignment;

public class UpdateAssignmentDto
{
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public int AssignmentListId { get; set; }
}