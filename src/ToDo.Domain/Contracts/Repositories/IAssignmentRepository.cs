using ToDo.Domain.Contracts.Interfaces;
using ToDo.Domain.Filter;
using ToDo.Domain.Models;

namespace ToDo.Domain.Contracts.Repositories;

public interface IAssignmentRepository : IEntityRepository<Assignment>
{
    Task<Assignment?> GetById(int id, int? userId);
    Task<IPagedResult<Assignment>> Search(int? userId, int assignmentListId, AssignmentFilter filter, int perPage = 10, 
        int page = 1);
}