using Domain.Entities.Task;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Tasks;

public class RecurrenceRuleConfiguration : IEntityTypeConfiguration<RecurrenceRule>
{
    public void Configure(EntityTypeBuilder<RecurrenceRule> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.TaskId)
            .IsRequired();

        builder.Property(r => r.RecurrenceType)
            .IsRequired();

        builder.Property(r => r.Interval)
            .IsRequired();

        builder.Property(r => r.WeekDays)
            .HasMaxLength(100);
    }
}
