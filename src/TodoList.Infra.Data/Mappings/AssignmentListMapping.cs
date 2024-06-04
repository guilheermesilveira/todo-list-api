using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.Models;

namespace TodoList.Infra.Data.Mappings;

public class AssignmentListMapping : IEntityTypeConfiguration<AssignmentList>
{
    public void Configure(EntityTypeBuilder<AssignmentList> builder)
    {
        builder
            .HasKey(assignmentList => assignmentList.Id);

        builder
            .Property(assignmentList => assignmentList.Name)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder
            .Property(assignmentList => assignmentList.UserId)
            .IsRequired();

        builder
            .Property(assignmentList => assignmentList.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(assignmentList => assignmentList.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");

        builder
            .HasMany(assignmentList => assignmentList.Assignments)
            .WithOne(assignment => assignment.AssignmentList)
            .OnDelete(DeleteBehavior.Cascade);
    }
}