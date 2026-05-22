using Application.Common.Result;
using Domain.Interfaces.Task;
using MediatR;

namespace Application.Features.Tasks.Commands;

public class DeleteTaskCommand : IRequest<Result>
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
}

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.TaskId);
        if (task == null || task.UserId != request.UserId)
            return Result.Failure("Task not found");

        await _taskRepository.DeleteAsync(request.TaskId);

        return Result.Success("Task deleted successfully");
    }
}
