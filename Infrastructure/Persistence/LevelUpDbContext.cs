using Domain.Entities.Journal;
using Domain.Entities.Notification;
using Domain.Entities.Streak;
using Domain.Entities.SubTask;
using Domain.Entities.User;
using Infrastructure.Persistence.Configurations.Journal;
using Infrastructure.Persistence.Configurations.Stats;
using Infrastructure.Persistence.Configurations.Tasks;
using Infrastructure.Persistence.Configurations.Users;
using Microsoft.EntityFrameworkCore;
using StatsEntity = Domain.Entities.Stats.Stats;
using TaskEntity = Domain.Entities.Task.Task;

namespace Infrastructure.Persistence;

public class LevelUpDbContext : DbContext
{
    public LevelUpDbContext(DbContextOptions<LevelUpDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
    public DbSet<SubTask> SubTasks => Set<SubTask>();
    public DbSet<Domain.Entities.Task.RecurrenceRule> RecurrenceRules => Set<Domain.Entities.Task.RecurrenceRule>();
    public DbSet<StatsEntity> Stats => Set<StatsEntity>();
    public DbSet<Journal> Journals => Set<Journal>();
    public DbSet<StreakHistory> StreakHistories => Set<StreakHistory>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new SubTaskConfiguration());
        modelBuilder.ApplyConfiguration(new RecurrenceRuleConfiguration());
        modelBuilder.ApplyConfiguration(new StatsConfiguration());
        modelBuilder.ApplyConfiguration(new JournalConfiguration());
        modelBuilder.ApplyConfiguration(new StreakHistoryConfiguration());
    }
}
