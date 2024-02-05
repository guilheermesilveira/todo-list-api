namespace TodoList.Domain.Models;

public class Assignment : Entity
{
    public string Description { get; set; } = null!;
    public DateTime Deadline { get; set; }
    public bool Concluded { get; private set; }
    public DateTime? ConcludedAt { get; private set; }
    public int UserId { get; set; }
    public int AssignmentListId { get; set; }

    // EF Relation
    public User User { get; set; } = null!;
    public AssignmentList AssignmentList { get; set; } = null!;

    public void SetConclude()
    {
        Concluded = true;
        ConcludedAt = DateTime.Now;
    }

    public void SetNotConclude()
    {
        Concluded = false;
        ConcludedAt = null;
    }
}