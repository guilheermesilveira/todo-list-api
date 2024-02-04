using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Contracts;
using TodoList.Domain.Models;

namespace TodoList.Infra.Data.Context;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<AssignmentList> AssignmentLists { get; set; } = null!;
    public DbSet<Assignment> Assignments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<bool> Commit()
    {
        return await SaveChangesAsync() > 0;
    }
}