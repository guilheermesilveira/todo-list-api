using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.Models;

namespace TodoList.Infra.Data.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasColumnType("VARCHAR(50)");

        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder
            .Property(x => x.Password)
            .IsRequired()
            .HasColumnType("VARCHAR(255)");

        builder
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(x => x.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        builder
            .HasMany(x => x.AssignmentLists)
            .WithOne(x => x.User);

        builder
            .HasMany(x => x.Assignments)
            .WithOne(x => x.User);
    }
}