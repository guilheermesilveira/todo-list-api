using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Contracts.Repositories;
using ToDo.Domain.Models;
using ToDo.Infra.Data.Context;

namespace ToDo.Infra.Data.Repositories;

public class UserRepository : EntityRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    { }

    public void Create(User user)
    {
        Context.Users.Add(user);
    }
    
    public async Task<User?> GetByEmail(string email)
    {
        return await Context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);
    }
}