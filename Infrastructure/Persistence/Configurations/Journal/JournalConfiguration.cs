using Domain.Entities.Journal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Journal;

public class JournalConfiguration : IEntityTypeConfiguration<Domain.Entities.Journal.Journal>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Journal.Journal> builder)
    {
        builder.HasKey(j => j.Id);

        builder.Property(j => j.UserId)
            .IsRequired();

        builder.Property(j => j.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(j => j.Date)
            .IsRequired();

        builder.Property(j => j.Category)
            .IsRequired();

        builder.Property(j => j.SubCategory)
            .HasMaxLength(100);

        builder.Property(j => j.Content)
            .IsRequired();

        builder.Property(j => j.CreatedAt)
            .IsRequired();

        builder.Property(j => j.UpdatedAt)
            .IsRequired();

        builder.HasIndex(j => j.Date);
    }
}
