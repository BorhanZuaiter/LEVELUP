using Application.Common.Result;
using Domain.Interfaces.Journal;
using MediatR;

namespace Application.Features.Journal.Commands;

public class DeleteJournalCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
}

public class DeleteJournalCommandHandler : IRequestHandler<DeleteJournalCommand, Result>
{
    private readonly IJournalRepository _journalRepository;

    public DeleteJournalCommandHandler(IJournalRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    public async global::System.Threading.Tasks.Task<Result> Handle(DeleteJournalCommand request, System.Threading.CancellationToken cancellationToken)
    {
        var journal = await _journalRepository.GetByIdAsync(request.Id);
        if (journal == null || journal.UserId != request.UserId)
            return Result.Failure("Journal entry not found");

        await _journalRepository.DeleteAsync(request.Id);
        return Result.Success("Journal entry deleted successfully");
    }
}
