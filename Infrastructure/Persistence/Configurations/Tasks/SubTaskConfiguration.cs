using Domain.Entities.SubTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Tasks;

public class SubTaskConfiguration : IEntityTypeConfiguration<SubTask>
{
    public void Configure(EntityTypeBuilder<SubTask> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TaskId)
            .IsRequired();

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(s => s.Order)
            .IsRequired();

        builder.Property(s => s.IsCompleted)
            .IsRequired();
    }
}
