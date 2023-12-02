using System.Linq.Expressions;
using ToDo.Domain.Contracts.Interfaces;
using ToDo.Domain.Models;

namespace ToDo.Domain.Contracts.Repositories;

public interface IEntityRepository<T> : IDisposable where T : Entity
{ 
    IUnitOfWork UnitOfWork { get; }
    Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression);
}