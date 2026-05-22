using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskEntity = Domain.Entities.Task.Task;

namespace Infrastructure.Persistence.Configurations.Tasks;

public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.UserId)
            .IsRequired();

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.Description)
            .HasMaxLength(2000);

        builder.Property(t => t.Difficulty)
            .IsRequired();

        builder.Property(t => t.Importance)
            .IsRequired();

        builder.Property(t => t.DurationMinutes)
            .IsRequired();

        builder.Property(t => t.TaskColor)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.StatCategory)
            .IsRequired();

        builder.Property(t => t.RecurrenceType)
            .IsRequired();

        builder.Property(t => t.TaskStatus)
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        builder.Property(t => t.IsRequired)
            .IsRequired();

        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => t.TaskStatus);

        builder.HasMany(t => t.SubTasks)
            .WithOne(s => s.Task)
            .HasForeignKey(s => s.TaskId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.RecurrenceRules)
            .WithOne(r => r.Task)
            .HasForeignKey(r => r.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
