using Application.Common.Result;
using Domain.Enums;
using Domain.Interfaces.Task;
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

    public CompleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null || task.UserId != request.UserId)
            return Result.Failure("Task not found");

        task.TaskStatus = Domain.Enums.TaskStatus.Completed;
        task.CompletedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);

        return Result.Success("Task completed successfully");
    }
}
