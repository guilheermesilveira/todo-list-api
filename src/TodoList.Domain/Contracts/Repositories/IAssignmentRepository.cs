using System.Linq.Expressions;
using TodoList.Domain.Filter;
using TodoList.Domain.Models;

namespace TodoList.Domain.Contracts.Repositories;

public interface IAssignmentRepository : IRepository<Assignment>
{
    void Create(Assignment assignment);
    void Update(Assignment assignment);
    void Delete(Assignment assignment);
    Task<Assignment?> FirstOrDefault(Expression<Func<Assignment, bool>> expression);
    Task<Assignment?> GetById(int id, int? userId);
    Task<IPagedResult<Assignment>> Search(int? userId, int? assignmentListId, AssignmentFilter filter, int perPage = 10,
        int page = 1);
}