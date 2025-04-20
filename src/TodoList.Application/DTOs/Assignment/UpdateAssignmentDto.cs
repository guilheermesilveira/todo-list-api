namespace TodoList.Application.DTOs.Assignment;

public class UpdateAssignmentDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public DateTime? Deadline { get; set; }
    public int AssignmentListId { get; set; }
}