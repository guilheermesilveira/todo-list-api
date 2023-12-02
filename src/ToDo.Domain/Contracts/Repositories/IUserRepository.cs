using ToDo.Domain.Models;

namespace ToDo.Domain.Contracts.Repositories;

public interface IUserRepository : IEntityRepository<User>
{ 
    void Create(User user);
    Task<User?> GetByEmail(string email);
}