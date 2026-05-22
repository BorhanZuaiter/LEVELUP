using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StatsEntity = Domain.Entities.Stats.Stats;

namespace Infrastructure.Persistence.Configurations.Stats;

public class StatsConfiguration : IEntityTypeConfiguration<StatsEntity>
{
    public void Configure(EntityTypeBuilder<StatsEntity> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.UserId)
            .IsRequired();

        builder.Property(s => s.Power)
            .IsRequired();

        builder.Property(s => s.Wisdom)
            .IsRequired();

        builder.Property(s => s.Luck)
            .IsRequired();

        builder.Property(s => s.Determination)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        builder.HasIndex(s => s.UserId)
            .IsUnique();
    }
}
