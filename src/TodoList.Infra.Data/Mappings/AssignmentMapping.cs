using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoList.Domain.Models;

namespace TodoList.Infra.Data.Mappings;

public class AssignmentMapping : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Description)
            .IsRequired()
            .HasColumnType("VARCHAR(200)");

        builder
            .Property(x => x.Deadline)
            .IsRequired(false)
            .HasColumnType("DATETIME");

        builder
            .Property(x => x.Concluded)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnType("TINYINT");

        builder
            .Property(x => x.ConcludedAt)
            .IsRequired(false)
            .HasColumnType("DATETIME");

        builder
            .Property(x => x.UserId)
            .IsRequired();

        builder
            .Property(x => x.AssignmentListId)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnType("DATETIME");

        builder
            .Property(x => x.UpdatedAt)
            .ValueGeneratedOnAddOrUpdate()
            .HasColumnType("DATETIME");
    }
}