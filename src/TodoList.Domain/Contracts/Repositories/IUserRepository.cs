using TodoList.Domain.Models;

namespace TodoList.Domain.Contracts.Repositories;

public interface IUserRepository : IRepository<User>
{
    void Create(User user);
    Task<User?> GetByEmail(string email);
}