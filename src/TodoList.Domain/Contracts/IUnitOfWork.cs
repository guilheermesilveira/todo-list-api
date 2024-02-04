namespace TodoList.Domain.Contracts;

public interface IUnitOfWork
{
    Task<bool> Commit();
}