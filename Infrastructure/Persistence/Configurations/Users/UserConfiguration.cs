using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(500);

        builder.Property(u => u.JoinDate)
            .IsRequired();

        builder.Property(u => u.EmailVerified)
            .IsRequired();

        builder.Property(u => u.EmailVerificationToken)
            .HasMaxLength(500);

        builder.Property(u => u.ForgotPasswordToken)
            .HasMaxLength(500);

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(500);

        builder.Property(u => u.Level)
            .IsRequired();

        builder.Property(u => u.XP)
            .IsRequired();

        builder.Property(u => u.CurrentHP)
            .IsRequired();

        builder.Property(u => u.MaxHP)
            .IsRequired();

        builder.Property(u => u.CurrentStreak)
            .IsRequired();

        builder.Property(u => u.ShieldCount)
            .IsRequired();

        builder.HasOne(u => u.Stats)
            .WithOne(s => s.User)
            .HasForeignKey<Domain.Entities.Stats.Stats>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Tasks)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Journals)
            .WithOne(j => j.User)
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.StreakHistories)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
