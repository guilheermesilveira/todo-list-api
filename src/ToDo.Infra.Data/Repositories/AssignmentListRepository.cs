using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Interfaces;
using ToDo.Domain.Contracts.Repositories;
using ToDo.Domain.Models;
using ToDo.Infra.Data.Context;
using ToDo.Infra.Data.Paged;

namespace ToDo.Infra.Data.Repositories;

public class AssignmentListRepository : EntityRepository<AssignmentList>, IAssignmentListRepository
{
    public AssignmentListRepository(ApplicationDbContext context) : base(context)
    { }

    public void Create(AssignmentList assignmentList)
    {
        Context.AssignmentLists.Add(assignmentList);
    }
    
    public void Update(AssignmentList assignmentList)
    {
        Context.AssignmentLists.Update(assignmentList);
    }
    
    public void Delete(AssignmentList assignmentList)
    {
        Context.AssignmentLists.Remove(assignmentList);
    }
    
    public async Task<AssignmentList?> GetById(int id, int? userId)
    {
        return await Context.AssignmentLists
            .AsNoTracking()
            .Include(x => x.Assignments)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }
    
    public async Task<IPagedResult<AssignmentList>> Search(int? userId, string name, int perPage = 10, int page = 1)
    {
        var query = Context.AssignmentLists
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name)) 
            query = query.Where(x => x.Name.Contains(name));
        
        var result = new PagedResult<AssignmentList>
        {
            Items = await query
                .OrderBy(x => x.Name)
                .Include(x => x.Assignments)
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
}