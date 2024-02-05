namespace TodoList.Application.DTOs.Assignment;

public class AssignmentDto
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public bool Concluded { get; set; }
    public DateTime? ConcludedAt { get; set; }
    public int UserId { get; set; }
    public int AssignmentListId { get; set; }
}