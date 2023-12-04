using ToDo.Application.DTOs.Assignment;

namespace ToDo.Application.DTOs.AssignmentList;

public class AssignmentListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual List<AssignmentDto> Assignments { get; set; } = new();
}