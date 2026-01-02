using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerApi.Domain.Entities;

namespace TaskManagerApi.Infrastructure.Configurations
{
    /// <summary>
    /// Entity Framework configuration for WorkItem entity.
    /// Defines table name, column names, constraints, and indexes.
    /// </summary>
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> builder)
        {
            builder.ToTable("workitems");

            // Primary Key
            builder.HasKey(w => w.Id);

            // Column configurations
            builder.Property(w => w.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd(); // Auto-increment

            builder.Property(w => w.Title)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(200); // Set max length for performance

            builder.Property(w => w.Description)
                .HasColumnName("description")
                .HasMaxLength(1000); // Optional with max length

            builder.Property(w => w.IsCompleted)
                .HasColumnName("is_completed")
                .IsRequired()
                .HasDefaultValue(false); // Default value in database

            builder.Property(w => w.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp without time zone");

            builder.Property(w => w.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp without time zone")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Auto-set on insert

            builder.Property(w => w.UserId)
                .HasColumnName("user_id");

            builder.HasOne(w => w.User)
                .WithMany(u => u.WorkItems)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for better performance
            builder.HasIndex(w => w.IsCompleted)
                .HasDatabaseName("idx_workitems_is_completed");

            builder.HasIndex(w => w.CreatedAt)
                .HasDatabaseName("idx_workitems_created_at");
        }
    }
}
