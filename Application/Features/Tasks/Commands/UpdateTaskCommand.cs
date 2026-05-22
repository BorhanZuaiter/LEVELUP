using Application.Common.Result;
using Application.DTOs.Tasks;
using Domain.Enums;
using Domain.Interfaces.Task;
using MediatR;
using Priority = Domain.Enums.Priority;

namespace Application.Features.Tasks.Commands;

public class UpdateTaskCommand : IRequest<Result<TaskDto>>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public UpdateTaskRequest Request { get; set; } = null!;
}

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, Result<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<TaskDto>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null || task.UserId != request.UserId)
            return Result.Failure<TaskDto>("Task not found");

        task.Title = request.Request.Title;
        task.Description = request.Request.Description;
        task.Difficulty = (Difficulty)request.Request.Difficulty;
        task.Importance = (Importance)request.Request.Importance;
        task.Priority = (Priority)request.Request.Priority;
        task.DurationMinutes = request.Request.DurationMinutes;
        task.TaskColor = request.Request.TaskColor;
        task.StatCategory = (StatCategory)request.Request.StatCategory;
        task.RecurrenceType = (RecurrenceType)request.Request.RecurrenceType;
        task.IsRequired = request.Request.IsRequired;
        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);

        var dto = MapToDto(task);
        return Result.Success(dto, "Task updated successfully");
    }

    private TaskDto MapToDto(Domain.Entities.Task.Task task)
    {
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Difficulty = (int)task.Difficulty,
            Importance = (int)task.Importance,
            Priority = (int)task.Priority,
            DurationMinutes = task.DurationMinutes,
            TaskColor = task.TaskColor,
            StatCategory = (int)task.StatCategory,
            RecurrenceType = (int)task.RecurrenceType,
            TaskStatus = (int)task.TaskStatus,
            IsRequired = task.IsRequired,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            CompletedAt = task.CompletedAt
        };
    }
}
