using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.Models;

namespace TodoList.Infra.Data.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.Name)
            .IsRequired()
            .HasColumnType("VARCHAR(50)");

        builder
            .Property(u => u.Email)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder
            .Property(u => u.Password)
            .IsRequired()
            .HasColumnType("VARCHAR(255)");

        builder
            .Property(u => u.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(u => u.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        builder
            .HasMany(u => u.AssignmentLists)
            .WithOne(assignmentList => assignmentList.User);

        builder
            .HasMany(u => u.Assignments)
            .WithOne(assignment => assignment.User);
    }
}