using TodoList.Domain.Models;

namespace TodoList.Domain.Contracts.Repositories;

public interface IRepository<T> : IDisposable where T : Entity
{
    IUnitOfWork UnitOfWork { get; }
}