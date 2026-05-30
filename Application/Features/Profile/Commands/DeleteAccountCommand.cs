using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Interfaces.Journal;
using Domain.Interfaces.Notification;
using Domain.Interfaces.Stats;
using Domain.Interfaces.Task;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Profile.Commands;

public class DeleteAccountCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string Password { get; set; } = string.Empty;
}

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IJournalRepository _journalRepository;
    private readonly IStatsRepository _statsRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IPasswordService _passwordService;

    public DeleteAccountCommandHandler(
        IUserRepository userRepository,
        ITaskRepository taskRepository,
        IJournalRepository journalRepository,
        IStatsRepository statsRepository,
        INotificationRepository notificationRepository,
        IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _taskRepository = taskRepository;
        _journalRepository = journalRepository;
        _statsRepository = statsRepository;
        _notificationRepository = notificationRepository;
        _passwordService = passwordService;
    }

    public async Task<Result> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return Result.Failure("User not found");

            // Verify password
            if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
                return Result.Failure("Invalid password. Account deletion cancelled.");

            // Delete all user data - cascade delete everything
            // This ensures GDPR compliance by removing all user data from the system

            // Delete all tasks for this user
            var userTasks = await _taskRepository.GetByUserIdAsync(request.UserId);
            foreach (var task in userTasks)
            {
                await _taskRepository.DeleteAsync(task.Id);
            }

            // Delete all journal entries for this user
            var userJournals = await _journalRepository.GetByUserIdAsync(request.UserId);
            foreach (var journal in userJournals)
            {
                await _journalRepository.DeleteAsync(journal.Id);
            }

            // Delete all stats for this user
            var userStats = await _statsRepository.GetByUserIdAsync(request.UserId);
            if (userStats != null)
            {
                await _statsRepository.DeleteAsync(userStats.Id);
            }

            // Delete all notifications for this user
            var userNotifications = await _notificationRepository.GetByUserIdAsync(request.UserId);
            foreach (var notification in userNotifications)
            {
                await _notificationRepository.DeleteAsync(notification.Id);
            }

            // Finally, delete the user account
            await _userRepository.DeleteAsync(request.UserId);

            return Result.Success("Account and all associated data deleted successfully");
        }
        catch (Exception ex)
        {
            // Log the exception in production
            return Result.Failure($"Failed to delete account: {ex.Message}");
        }
    }
}
