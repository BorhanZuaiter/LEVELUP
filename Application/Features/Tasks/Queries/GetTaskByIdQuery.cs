using Application.Common.Result;
using Application.DTOs.Tasks;
using Domain.Interfaces.Task;
using MediatR;

namespace Application.Features.Tasks.Queries;

public class GetTaskByIdQuery : IRequest<Result<TaskDto>>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
}

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<TaskDto>>
{
    private readonly ITaskRepository _taskRepository;

    public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null || task.UserId != request.UserId)
            return Result.Failure<TaskDto>("Task not found");

        var dto = MapToDto(task);
        return Result.Success(dto);
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
