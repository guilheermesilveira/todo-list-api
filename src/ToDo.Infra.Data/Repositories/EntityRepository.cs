using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Interfaces;
using ToDo.Domain.Contracts.Repositories;
using ToDo.Domain.Models;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repositories;

public abstract class EntityRepository<T> : IEntityRepository<T> where T : Entity
{
    protected readonly ApplicationDbContext Context;
    private readonly DbSet<T> _dbSet;
    private bool _isDisposed;

    protected EntityRepository(ApplicationDbContext context)
    {
        Context = context;
        _dbSet = context.Set<T>();
    }
    
    public IUnitOfWork UnitOfWork => Context;
    
    public async Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AsNoTrackingWithIdentityResolution().Where(expression).FirstOrDefaultAsync();
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) 
            return;

        if (disposing) 
            Context.Dispose();

        _isDisposed = true;
    }
    
    ~EntityRepository()
    {
        Dispose(false);
    }
}