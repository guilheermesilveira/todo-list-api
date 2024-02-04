using TodoList.Application.DTOs.Assignment;
using TodoList.Application.DTOs.AssignmentList;
using TodoList.Application.DTOs.Paged;

namespace TodoList.Application.Contracts.Services;

public interface IAssignmentListService
{
    Task<AssignmentListDto?> Create(CreateAssignmentListDto dto);
    Task<AssignmentListDto?> Update(int id, UpdateAssignmentListDto dto);
    Task Delete(int id);
    Task<AssignmentListDto?> GetById(int id);
    Task<PagedDto<AssignmentListDto>> Search(SearchAssignmentListDto dto);
    Task<PagedDto<AssignmentDto>?> SearchAssignments(int id, SearchAssignmentDto dto);
}