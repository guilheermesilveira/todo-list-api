namespace TodoList.Application.DTOs.AssignmentList;

public class SearchAssignmentListDto
{
    public string? Name { get; set; }
    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 10;
}