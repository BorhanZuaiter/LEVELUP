using Domain.Entities.Streak;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Journal;

public class StreakHistoryConfiguration : IEntityTypeConfiguration<StreakHistory>
{
    public void Configure(EntityTypeBuilder<StreakHistory> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.UserId)
            .IsRequired();

        builder.Property(s => s.Date)
            .IsRequired();

        builder.Property(s => s.StreakCount)
            .IsRequired();

        builder.Property(s => s.ShieldUsed)
            .IsRequired();

        builder.HasIndex(s => s.Date);
    }
}
