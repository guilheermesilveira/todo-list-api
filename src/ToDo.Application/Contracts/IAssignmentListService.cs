using ToDo.Application.DTOs.Assignment;
using ToDo.Application.DTOs.AssignmentList;
using ToDo.Application.DTOs.Paged;

namespace ToDo.Application.Contracts;

public interface IAssignmentListService
{
    Task<AssignmentListDto?> Create(CreateAssignmentListDto dto);
    Task<AssignmentListDto?> Update(int id, UpdateAssignmentListDto dto);
    Task Delete(int id);
    Task<AssignmentListDto?> GetById(int id);
    Task<PagedDto<AssignmentListDto>> Search(SearchAssignmentListDto dto);
    Task<PagedDto<AssignmentDto>?> SearchAssignments(int id, SearchAssignmentDto dto);
}