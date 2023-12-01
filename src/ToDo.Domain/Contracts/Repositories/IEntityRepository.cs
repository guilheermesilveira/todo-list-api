using System.Linq.Expressions;
using ToDo.Domain.Contracts.Interfaces;
using ToDo.Domain.Models;

namespace ToDo.Domain.Contracts.Repositories;

public interface IEntityRepository<T> : IDisposable where T : Entity
{
    IUnitOfWork UnityOfWork { get; }

    void Create(T entity);
    void Update(T entity);
    void Delete(T entity); 
    Task<T?> FirstOrDefault(Expression<Func<T, bool>> expression);
}