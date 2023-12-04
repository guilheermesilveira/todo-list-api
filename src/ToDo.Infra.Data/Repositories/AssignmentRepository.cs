using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Interfaces;
using ToDo.Domain.Contracts.Repositories;
using ToDo.Domain.Filter;
using ToDo.Domain.Models;
using ToDo.Infra.Data.Context;
using ToDo.Infra.Data.Paged;

namespace ToDo.Infra.Data.Repositories;

public class AssignmentRepository : EntityRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(ApplicationDbContext context) : base(context)
    { }
    
    public void Create(Assignment assignment)
    {
        Context.Assignments.Add(assignment);
    }
    
    public void Update(Assignment assignment)
    {
        Context.Assignments.Update(assignment);
    }
    
    public void Delete(Assignment assignment)
    {
        Context.Assignments.Remove(assignment);
    }
    
    public async Task<Assignment?> GetById(int id, int? userId)
    {
        return await Context.Assignments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
    }
    
    public async Task<IPagedResult<Assignment>> Search(int? userId, int? assignmentListId, AssignmentFilter filter, 
        int perPage = 10, int page = 1)
    {
        var query = Context.Assignments
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
}