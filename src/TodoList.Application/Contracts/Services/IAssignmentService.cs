using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.Paged;

namespace TodoList.Application.Contracts.Services;

public interface IAssignmentService
{
    Task<AssignmentDto?> Create(CreateAssignmentDto dto);
    Task<AssignmentDto?> Update(int id, UpdateAssignmentDto dto);
    Task MarkConclude(int id);
    Task MarkNotConclude(int id);
    Task Delete(int id);
    Task<AssignmentDto?> GetById(int id);
    Task<PagedDto<AssignmentDto>> Search(SearchAssignmentDto dto);
}