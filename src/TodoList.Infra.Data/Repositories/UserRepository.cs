using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Contracts;
using TodoList.Domain.Contracts.Repositories;
using TodoList.Domain.Models;
using TodoList.Infra.Data.Context;

namespace TodoList.Infra.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public void Register(User user)
    {
        _context.Users.Add(user);
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}