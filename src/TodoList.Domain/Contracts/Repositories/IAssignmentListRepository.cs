using System.Linq.Expressions;
using TodoList.Domain.Models;

namespace TodoList.Domain.Contracts.Repositories;

public interface IAssignmentListRepository : IRepository<AssignmentList>
{
    void Create(AssignmentList assignmentList);
    void Update(AssignmentList assignmentList);
    void Delete(AssignmentList assignmentList);
    Task<AssignmentList?> FirstOrDefault(Expression<Func<AssignmentList, bool>> expression);
    Task<AssignmentList?> GetById(int id, int? userId);
    Task<IPagedResult<AssignmentList>> Search(int? userId, string? name, int perPage = 10, int page = 1);
}