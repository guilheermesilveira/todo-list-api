using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.Models;

namespace TodoList.Infra.Data.Mappings;

public class AssignmentMapping : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder
            .HasKey(assignment => assignment.Id);

        builder
            .Property(assignment => assignment.Description)
            .IsRequired()
            .HasColumnType("VARCHAR(200)");

        builder
            .Property(assignment => assignment.Deadline)
            .IsRequired(false)
            .HasColumnType("DATETIME");

        builder
            .Property(assignment => assignment.Concluded)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnType("TINYINT");

        builder
            .Property(assignment => assignment.ConcludedAt)
            .IsRequired(false)
            .HasColumnType("DATETIME");

        builder
            .Property(assignment => assignment.UserId)
            .IsRequired();

        builder
            .Property(assignment => assignment.AssignmentListId)
            .IsRequired();

        builder
            .Property(assignment => assignment.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(assignment => assignment.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");
    }
}