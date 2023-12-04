using ToDo.Application.DTOs.Assignment;
using ToDo.Application.DTOs.Paged;

namespace ToDo.Application.Contracts;

public interface IAssignmentService
{
    Task<AssignmentDto?> Create(CreateAssignmentDto dto);
    Task<AssignmentDto?> Update(int id, UpdateAssignmentDto dto);
    Task Delete(int id);
    Task<AssignmentDto?> GetById(int id);
    Task<PagedDto<AssignmentDto>> Search(SearchAssignmentDto dto);
    Task MarkConcluded(int id);
    Task MarkDesconcluded(int id);
}