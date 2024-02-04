using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.Models;

namespace TodoList.Infra.Data.Mappings;

public class AssignmentListMapping : IEntityTypeConfiguration<AssignmentList>
{
    public void Configure(EntityTypeBuilder<AssignmentList> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder
            .Property(x => x.UserId)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(x => x.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        builder
            .HasMany(x => x.Assignments)
            .WithOne(x => x.AssignmentList)
            .OnDelete(DeleteBehavior.Cascade);
    }
}