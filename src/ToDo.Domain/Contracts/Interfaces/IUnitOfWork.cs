namespace ToDo.Domain.Contracts.Interfaces;

public interface IUnitOfWork
{
    Task<bool> Commit();
}