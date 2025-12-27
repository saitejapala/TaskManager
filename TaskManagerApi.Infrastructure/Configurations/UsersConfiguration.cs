using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagerApi.Domain.Entities;

namespace TaskManagerApi.Infrastructure.Configurations
{
    /// <summary>
    /// Entity Framework configuration for WorkItem entity.
    /// Defines table name, column names, constraints, and indexes.
    /// </summary>
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("users");

            // Primary Key
            builder.HasKey(w => w.UserId);

            // Column configurations
            builder.Property(w => w.UserId)
                .HasColumnName("user_id")
                .ValueGeneratedOnAdd(); // Auto-increment

            builder.Property(w => w.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(200); // Set max length for performance

            builder.Property(w => w.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired()
                .HasColumnType("TEXT");

            builder.Property(w => w.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(250); // Optional with max length

            builder.Property(w => w.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp without time zone");

            builder.Property(w => w.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp without time zone")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Auto-set on insert
        }
    }
}
