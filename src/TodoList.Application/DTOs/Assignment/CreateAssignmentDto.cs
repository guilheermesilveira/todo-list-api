namespace TodoList.Application.DTOs.Assignment;

public class CreateAssignmentDto
{
    public string Description { get; set; } = null!;
    public DateTime? Deadline { get; set; }
    public int AssignmentListId { get; set; }
}