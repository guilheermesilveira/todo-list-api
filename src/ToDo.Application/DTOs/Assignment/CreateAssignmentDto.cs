namespace ToDo.Application.DTOs.Assignment;

public class CreateAssignmentDto
{
    public int AssignmentListId { get; set; }
    public string Description { get; set; } = null!;
    public DateTime? Deadline { get; set; } 
}