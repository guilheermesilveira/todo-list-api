using ToDo.Domain.Models;

namespace ToDo.Domain.Contracts.Repositories;

public interface IUserRepository : IEntityRepository<User>
{ 
    Task<User?> GetByEmail(string email);
}