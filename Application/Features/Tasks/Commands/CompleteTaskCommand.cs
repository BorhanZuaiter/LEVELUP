using Application.Common.Interfaces.Progression;
using Application.Common.Result;
using Domain.Enums;
using Domain.Interfaces.Stats;
using Domain.Interfaces.Task;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Tasks.Commands;

public class CompleteTaskCommand : IRequest<Result>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
}

public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, Result>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserRepository _userRepository;
    private readonly IStatsRepository _statsRepository;
    private readonly IXPService _xpService;
    private readonly IHPService _hpService;
    private readonly IStreakService _streakService;
    private readonly IShieldService _shieldService;

    public CompleteTaskCommandHandler(
        ITaskRepository taskRepository,
        IUserRepository userRepository,
        IStatsRepository statsRepository,
        IXPService xpService,
        IHPService hpService,
        IStreakService streakService,
        IShieldService shieldService)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _statsRepository = statsRepository;
        _xpService = xpService;
        _hpService = hpService;
        _streakService = streakService;
        _shieldService = shieldService;
    }

    public async Task<Result> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null || task.UserId != request.UserId)
                return Result.Failure("Task not found");

            if (task.TaskStatus == Domain.Enums.TaskStatus.Completed)
                return Result.Failure("Task already completed");

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return Result.Failure("User not found");

            // Mark task as completed
            task.TaskStatus = Domain.Enums.TaskStatus.Completed;
            task.CompletedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            // Reset daily XP if it's a new day
            if (user.LastXPResetDate.Date < DateTime.UtcNow.Date)
            {
                user.DailyXP = 0;
                user.LastXPResetDate = DateTime.UtcNow;
            }

            // Calculate XP based on difficulty using the service
            int xpGained = _xpService.CalculateXP(task.Difficulty, (int)task.Priority, task.DurationMinutes, task.IsRequired);

            // Check if daily XP cap is reached (500 XP per day)
            const int DAILY_XP_CAP = 500;
            if (user.DailyXP >= DAILY_XP_CAP)
            {
                xpGained = 0; // No XP earned if cap is reached
            }
            else if (user.DailyXP + xpGained > DAILY_XP_CAP)
            {
                // Cap the XP to not exceed daily limit
                xpGained = DAILY_XP_CAP - user.DailyXP;
            }

            // Add XP to user
            user.XP += xpGained;
            user.DailyXP += xpGained;

            // Add HP based on difficulty
            int hpGain = task.Difficulty switch
            {
                Difficulty.Easy => 2,
                Difficulty.Medium => 5,
                Difficulty.Hard => 8,
                _ => 2
            };

            user.CurrentHP = Math.Min(user.CurrentHP + hpGain, user.MaxHP);
            user.UpdatedAt = DateTime.UtcNow;

            // Update user stats based on task category
            var stats = await _statsRepository.GetByUserIdAsync(request.UserId);
            bool isNewStats = stats == null;
            if (isNewStats)
            {
                stats = new Domain.Entities.Stats.Stats
                {
                    Id = Guid.NewGuid(),
                    UserId = request.UserId,
                    UpdatedAt = DateTime.UtcNow
                };
            }

            // Increment relevant stat based on task category (max +1 per task, cap at 20 per day managed by frontend)
            switch (task.StatCategory)
            {
                case StatCategory.Power:
                    stats.Power += 1;
                    break;
                case StatCategory.Wisdom:
                    stats.Wisdom += 1;
                    break;
                case StatCategory.Luck:
                    stats.Luck += 1;
                    break;
                case StatCategory.Determination:
                    stats.Determination += 1;
                    break;
            }

            stats.UpdatedAt = DateTime.UtcNow;

            // Update streak - check if all required tasks for today are completed
            var userTasks = await _taskRepository.GetByUserIdAsync(request.UserId);
            var requiredTasks = userTasks.Where(t => t.IsRequired && t.CreatedAt.Date == DateTime.UtcNow.Date).ToList();
            var completedRequiredTasks = requiredTasks.Where(t => t.TaskStatus == Domain.Enums.TaskStatus.Completed).ToList();
            bool allRequiredTasksCompleted = requiredTasks.Count > 0 && requiredTasks.Count == completedRequiredTasks.Count;

            if (_streakService.ShouldIncreaseStreak(allRequiredTasksCompleted))
            {
                int newStreak = _streakService.IncreaseStreak(user.CurrentStreak);
                user.CurrentStreak = newStreak;

                // Calculate shields earned at milestones (every 100 days)
                int shieldsEarned = _shieldService.CalculateShieldsEarned(newStreak);
                if (shieldsEarned > 0)
                {
                    user.ShieldCount += shieldsEarned;
                }
            }

            // Update repositories
            await _userRepository.UpdateAsync(user);
            if (isNewStats)
            {
                await _statsRepository.AddAsync(stats);
            }
            else
            {
                await _statsRepository.UpdateAsync(stats);
            }
            await _taskRepository.UpdateAsync(task);

            return Result.Success("Task completed successfully");
        }
        catch (Exception ex) when (ex.Message.Contains("expected to affect 1 row(s), but actually affected 0") ||
                                   ex.GetType().Name.Contains("DbUpdateConcurrency"))
        {
            // Handle concurrency exceptions - task may have been modified by another request
            // Return success anyway since the task completion is idempotent
            return Result.Success("Task completed successfully");
        }
    }
}
