using Application.Common.Result;
using Application.DTOs.Tasks;
using Domain.Interfaces.Task;
using MediatR;

namespace Application.Features.Tasks.Queries;

public class GetRequiredTasksQuery : IRequest<Result<List<TaskDto>>>
{
    public Guid UserId { get; set; }
}

public class GetRequiredTasksQueryHandler : IRequestHandler<GetRequiredTasksQuery, Result<List<TaskDto>>>
{
    private readonly ITaskRepository _taskRepository;

    public GetRequiredTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<List<TaskDto>>> Handle(GetRequiredTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetRequiredTasksAsync(request.UserId);
        var dtos = tasks.Select(MapToDto).ToList();
        return Result.Success(dtos);
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
