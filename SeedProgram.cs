using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Domain.Entities.User;
using Domain.Entities.Task;
using Domain.Entities.Journal;
using Domain.Enums;
using Infrastructure.Services;

Console.WriteLine("🌱 Starting database seeding...");

var options = new DbContextOptionsBuilder<LevelUpDbContext>()
    .UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LevelUpDB;Trusted_Connection=True;TrustServerCertificate=True;")
    .Options;

using var context = new LevelUpDbContext(options);

// Check if test user already exists
var testUserEmail = "test@levelup.com";
var existingUser = context.Users.FirstOrDefault(u => u.Email == testUserEmail);

if (existingUser != null)
{
    Console.WriteLine("ℹ️  Test user already exists. Clearing old data...");
    context.Journals.RemoveRange(context.Journals.Where(j => j.UserId == existingUser.Id));
    context.Tasks.RemoveRange(context.Tasks.Where(t => t.UserId == existingUser.Id));
    context.Users.Remove(existingUser);
    context.SaveChanges();
}

Console.WriteLine("👤 Creating test user...");
var passwordService = new PasswordService();
var userId = Guid.NewGuid();

var user = new User
{
    Id = userId,
    Username = "TestPlayer",
    Email = testUserEmail,
    PasswordHash = passwordService.HashPassword("Test@1234"),
    EmailVerified = true,
    JoinDate = DateTime.UtcNow.AddDays(-30),
    UpdatedAt = DateTime.UtcNow,
    Level = 5,
    XP = 250,
    CurrentHP = 85,
    MaxHP = 100,
    CurrentStreak = 7,
    ShieldCount = 0
};

context.Users.Add(user);
context.SaveChanges();

Console.WriteLine("📋 Creating test tasks...");
var tasks = new List<Task>
{
    new Task
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = "Morning Meditation",
        Description = "Meditate for 15 minutes",
        Difficulty = Difficulty.Easy,
        Importance = Importance.High,
        Priority = Priority.High,
        DurationMinutes = 15,
        TaskColor = "#FF6B6B",
        StatCategory = StatCategory.Wisdom,
        RecurrenceType = RecurrenceType.Daily,
        TaskStatus = TaskStatus.Completed,
        CreatedAt = DateTime.UtcNow.AddDays(-5),
        UpdatedAt = DateTime.UtcNow,
        CompletedAt = DateTime.UtcNow.AddHours(-2),
        IsRequired = true,
        ReminderEnabled = true,
        ReminderTime = new TimeSpan(6, 0, 0)
    },
    new Task
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = "Complete coding project",
        Description = "Finish the authentication module",
        Difficulty = Difficulty.Hard,
        Importance = Importance.High,
        Priority = Priority.High,
        DurationMinutes = 120,
        TaskColor = "#4ECDC4",
        StatCategory = StatCategory.Determination,
        RecurrenceType = RecurrenceType.Once,
        TaskStatus = TaskStatus.Pending,
        CreatedAt = DateTime.UtcNow.AddDays(-3),
        UpdatedAt = DateTime.UtcNow,
        IsRequired = false,
        ReminderEnabled = true,
        ReminderTime = new TimeSpan(9, 0, 0)
    },
    new Task
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = "Exercise",
        Description = "Go for a 30 minute run",
        Difficulty = Difficulty.Medium,
        Importance = Importance.Medium,
        Priority = Priority.Medium,
        DurationMinutes = 30,
        TaskColor = "#95E1D3",
        StatCategory = StatCategory.Power,
        RecurrenceType = RecurrenceType.Daily,
        TaskStatus = TaskStatus.Completed,
        CreatedAt = DateTime.UtcNow.AddDays(-2),
        UpdatedAt = DateTime.UtcNow,
        CompletedAt = DateTime.UtcNow.AddHours(-4),
        IsRequired = true,
        ReminderEnabled = false
    },
    new Task
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = "Read a chapter",
        Description = "Read Chapter 5 of Clean Code",
        Difficulty = Difficulty.Easy,
        Importance = Importance.Medium,
        Priority = Priority.Low,
        DurationMinutes = 45,
        TaskColor = "#F4A261",
        StatCategory = StatCategory.Wisdom,
        RecurrenceType = RecurrenceType.Weekly,
        TaskStatus = TaskStatus.Pending,
        CreatedAt = DateTime.UtcNow.AddDays(-1),
        UpdatedAt = DateTime.UtcNow,
        IsRequired = false,
        ReminderEnabled = false
    },
    new Task
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = "Drink water",
        Description = "Stay hydrated throughout the day",
        Difficulty = Difficulty.Easy,
        Importance = Importance.Low,
        Priority = Priority.Medium,
        DurationMinutes = 5,
        TaskColor = "#2A9D8F",
        StatCategory = StatCategory.Luck,
        RecurrenceType = RecurrenceType.Daily,
        TaskStatus = TaskStatus.Completed,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        CompletedAt = DateTime.UtcNow.AddHours(-1),
        IsRequired = true,
        ReminderEnabled = true,
        ReminderTime = new TimeSpan(12, 0, 0)
    }
};

context.Tasks.AddRange(tasks);
context.SaveChanges();

Console.WriteLine("📓 Creating test journals...");
var journals = new List<Journal>
{
    new Journal
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = "Great Day!",
        Content = "Had an amazing day! Completed all my tasks and felt very productive. The meditation session really helped calm my mind.",
        Category = JournalCategory.Positives,
        Date = DateTime.UtcNow.AddDays(-2),
        CreatedAt = DateTime.UtcNow.AddDays(-2),
        UpdatedAt = DateTime.UtcNow.AddDays(-2)
    },
    new Journal
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = "Challenges today",
        Content = "Found it hard to focus on coding today. Maybe I need more breaks during work sessions.",
        Category = JournalCategory.Negatives,
        Date = DateTime.UtcNow.AddDays(-1),
        CreatedAt = DateTime.UtcNow.AddDays(-1),
        UpdatedAt = DateTime.UtcNow.AddDays(-1)
    },
    new Journal
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = "Learning Goals",
        Content = "This month I want to:\n1. Finish the authentication module\n2. Learn advanced Flutter concepts\n3. Improve my morning routine consistency\n4. Build a habit of daily reflection",
        Category = JournalCategory.Goals,
        Date = DateTime.UtcNow.AddDays(-5),
        CreatedAt = DateTime.UtcNow.AddDays(-5),
        UpdatedAt = DateTime.UtcNow.AddDays(-5)
    },
    new Journal
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        Title = "Childhood Memory",
        Content = "Remembered the time when I first learned to code. It was challenging but so rewarding. Now I'm building an app that could help millions!",
        Category = JournalCategory.Memories,
        Date = DateTime.UtcNow,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    }
};

context.Journals.AddRange(journals);
context.SaveChanges();

Console.WriteLine("\n✅ Test data created successfully!\n");
Console.WriteLine("📊 Database Summary:");
Console.WriteLine($"   User: {user.Email}");
Console.WriteLine($"   Password: Test@1234");
Console.WriteLine($"   Username: {user.Username}");
Console.WriteLine($"   Level: {user.Level}");
Console.WriteLine($"   XP: {user.XP}");
Console.WriteLine($"   HP: {user.CurrentHP}/{user.MaxHP}");
Console.WriteLine($"   Streak: {user.CurrentStreak} days");
Console.WriteLine($"   Tasks Created: {tasks.Count}");
Console.WriteLine($"   Journals Created: {journals.Count}");
Console.WriteLine("\n🚀 You can now test the app!");
