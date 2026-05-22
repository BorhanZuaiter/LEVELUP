using Application.Common.Result;
using Application.DTOs.Tasks;
using Domain.Interfaces.Task;
using MediatR;

namespace Application.Features.Tasks.Queries;

public class SearchTasksQuery : IRequest<Result<List<TaskDto>>>
{
    public Guid UserId { get; set; }
    public SearchTasksRequest Request { get; set; } = null!;
}

public class SearchTasksQueryHandler : IRequestHandler<SearchTasksQuery, Result<List<TaskDto>>>
{
    private readonly ITaskRepository _taskRepository;

    public SearchTasksQueryHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result<List<TaskDto>>> Handle(SearchTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _taskRepository.SearchAsync(request.UserId, request.Request.Query ?? string.Empty);

        if (request.Request.Color.HasValue)
            tasks = tasks.Where(t => t.TaskColor == request.Request.Color.ToString()).ToList();

        if (request.Request.StatCategory.HasValue)
            tasks = tasks.Where(t => (int)t.StatCategory == request.Request.StatCategory).ToList();

        if (request.Request.Status.HasValue)
            tasks = tasks.Where(t => (int)t.TaskStatus == request.Request.Status).ToList();

        if (request.Request.IsRequired.HasValue)
            tasks = tasks.Where(t => t.IsRequired == request.Request.IsRequired).ToList();

        if (request.Request.Priority.HasValue)
            tasks = tasks.Where(t => (int)t.Priority == request.Request.Priority).ToList();

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
