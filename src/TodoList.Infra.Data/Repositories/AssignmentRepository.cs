using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Contracts;
using TodoList.Domain.Contracts.Repositories;
using TodoList.Domain.Filter;
using TodoList.Domain.Models;
using TodoList.Infra.Data.Context;
using TodoList.Infra.Data.Paged;

namespace TodoList.Infra.Data.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly ApplicationDbContext _context;

    public AssignmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Create(Assignment assignment)
    {
        _context.Assignments.Add(assignment);
    }

    public void Update(Assignment assignment)
    {
        _context.Assignments.Update(assignment);
    }

    public void Delete(Assignment assignment)
    {
        _context.Assignments.Remove(assignment);
    }

    public async Task<Assignment?> FirstOrDefault(Expression<Func<Assignment, bool>> expression)
    {
        return await _context.Assignments.AsNoTrackingWithIdentityResolution().Where(expression).FirstOrDefaultAsync();
    }

    public async Task<Assignment?> GetById(int id, int? userId)
    {
        return await _context.Assignments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }

    public async Task<IPagedResult<Assignment>> Search(int? userId, int? assignmentListId, AssignmentFilter filter,
        int perPage = 10, int page = 1)
    {
        var query = _context.Assignments
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .AsQueryable();

        ApplyFilter(userId, assignmentListId, filter, ref query);

        var result = new PagedResult<Assignment>
        {
            Items = await query.Skip((page - 1) * perPage).Take(perPage).ToListAsync(),
            Total = await query.CountAsync(),
            Page = page,
            PerPage = perPage
        };

        var pageCount = (double)result.Total / perPage;
        result.PageCount = (int)Math.Ceiling(pageCount);

        return result;
    }

    private static void ApplyFilter(int? userId, int? assignmentListId, AssignmentFilter filter,
        ref IQueryable<Assignment> query)
    {
        if (!string.IsNullOrWhiteSpace(filter.Description))
            query = query.Where(x => x.Description.Contains(filter.Description));

        if (filter.Concluded.HasValue)
            query = query.Where(x => x.Concluded == filter.Concluded.Value);

        if (filter.StartDeadline.HasValue)
            query = query.Where(x => x.Deadline >= filter.StartDeadline.Value);

        if (filter.EndDeadline.HasValue)
            query = query.Where(x => x.Deadline <= filter.EndDeadline.Value);

        if (assignmentListId.HasValue)
        {
            query = query
                .Where(x => x.AssignmentListId == assignmentListId)
                .Where(x => x.AssignmentList.UserId == userId);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}