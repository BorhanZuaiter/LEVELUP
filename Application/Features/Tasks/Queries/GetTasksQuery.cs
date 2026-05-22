using Application.Common.Result;
using Application.DTOs.Tasks;
using Domain.Interfaces.Task;
using MediatR;

namespace Application.Features.Tasks.Queries;

public class GetTasksQuery : IRequest<Result<List<TaskDto>>>
{
    public Guid UserId { get; set; }
    public TaskFilterCriteria? FilterCriteria { get; set; }
}

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Result<List<TaskDto>>>
{
    private readonly ITaskRepository _taskRepository;

    public GetTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<List<TaskDto>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.GetByUserIdAsync(request.UserId);

        if (request.FilterCriteria != null)
        {
            tasks = _taskRepository.FilterAsync(tasks, request.FilterCriteria).Result;
        }

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
