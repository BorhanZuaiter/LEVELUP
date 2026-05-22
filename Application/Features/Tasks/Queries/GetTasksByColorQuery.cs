using Application.Common.Result;
using Application.DTOs.Tasks;
using Domain.Interfaces.Task;
using MediatR;

namespace Application.Features.Tasks.Queries;

public class GetTasksByColorQuery : IRequest<Result<List<TaskDto>>>
{
    public Guid UserId { get; set; }
    public int Color { get; set; }
}

public class GetTasksByColorQueryHandler : IRequestHandler<GetTasksByColorQuery, Result<List<TaskDto>>>
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksByColorQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<List<TaskDto>>> Handle(GetTasksByColorQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetByColorAsync(request.UserId, request.Color);
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
