using Application.Common.Result;
using Application.DTOs.Tasks;
using Domain.Entities.Task;
using Domain.Enums;
using Domain.Interfaces.Task;
using MediatR;
using Priority = Domain.Enums.Priority;

namespace Application.Features.Tasks.Commands;

public class CreateTaskCommand : IRequest<Result<TaskDto>>
{
    public Guid UserId { get; set; }
    public CreateTaskRequest Request { get; set; } = null!;
}

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;

    public CreateTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new Domain.Entities.Task.Task
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Request.Title,
            Description = request.Request.Description,
            Difficulty = (Difficulty)request.Request.Difficulty,
            Importance = (Importance)request.Request.Importance,
            Priority = (Priority)request.Request.Priority,
            DurationMinutes = request.Request.DurationMinutes,
            TaskColor = request.Request.TaskColor,
            StatCategory = (StatCategory)request.Request.StatCategory,
            RecurrenceType = (RecurrenceType)request.Request.RecurrenceType,
            TaskStatus = Domain.Enums.TaskStatus.Pending,
            IsRequired = request.Request.IsRequired,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _taskRepository.AddAsync(task);

        var dto = MapToDto(task);
        return Result.Success(dto, "Task created successfully");
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
