using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Contracts;
using TodoList.Domain.Contracts.Repositories;
using TodoList.Domain.Models;
using TodoList.Infra.Data.Context;
using TodoList.Infra.Data.Paged;

namespace TodoList.Infra.Data.Repositories;

public class AssignmentListRepository : IAssignmentListRepository
{
    private readonly ApplicationDbContext _context;

    public AssignmentListRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Create(AssignmentList assignmentList)
    {
        _context.AssignmentLists.Add(assignmentList);
    }

    public void Update(AssignmentList assignmentList)
    {
        _context.AssignmentLists.Update(assignmentList);
    }

    public void Delete(AssignmentList assignmentList)
    {
        _context.AssignmentLists.Remove(assignmentList);
    }

    public async Task<AssignmentList?> FirstOrDefault(Expression<Func<AssignmentList, bool>> expression)
    {
        return await _context.AssignmentLists
            .AsNoTrackingWithIdentityResolution()
            .Where(expression)
            .FirstOrDefaultAsync();
    }

    public async Task<AssignmentList?> GetById(int id, int? userId)
    {
        return await _context.AssignmentLists
            .AsNoTracking()
            .Include(assignmentList => assignmentList.Assignments)
            .FirstOrDefaultAsync(assignmentList => assignmentList.Id == id && assignmentList.UserId == userId);
    }

    public async Task<IPagedResult<AssignmentList>> Search(int? userId, string? name, int perPage = 10, int page = 1)
    {
        var query = _context.AssignmentLists
            .AsNoTracking()
            .Where(assignmentList => assignmentList.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(assignmentList => assignmentList.Name.Contains(name));

        var result = new PagedResult<AssignmentList>
        {
            Items = await query
                .OrderBy(assignmentList => assignmentList.Name)
                .Include(assignmentList => assignmentList.Assignments)
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .ToListAsync(),
            Total = await query.CountAsync(),
            Page = page,
            PerPage = perPage
        };

        var pageCount = (double)result.Total / perPage;
        result.PageCount = (int)Math.Ceiling(pageCount);

        return result;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}