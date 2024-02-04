using System.Collections.ObjectModel;
using TodoList.Application.DTOs.Assignment;

namespace TodoList.Application.DTOs.AssignmentList;

public class AssignmentListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int UserId { get; set; }
    public virtual Collection<AssignmentDto> Assignments { get; set; } = new();
}