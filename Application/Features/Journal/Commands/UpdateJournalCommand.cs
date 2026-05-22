using Application.Common.Result;
using Application.DTOs.Journal;
using Domain.Enums;
using Domain.Interfaces.Journal;
using MediatR;

namespace Application.Features.Journal.Commands;

public class UpdateJournalCommand : IRequest<Result<JournalDto>>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public UpdateJournalRequest Request { get; set; } = null!;
}

public class UpdateJournalCommandHandler : IRequestHandler<UpdateJournalCommand, Result<JournalDto>>
{
    private readonly IJournalRepository _journalRepository;

    public UpdateJournalCommandHandler(IJournalRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<JournalDto>> Handle(UpdateJournalCommand request, System.Threading.CancellationToken cancellationToken)
    {
        var journal = await _journalRepository.GetByIdAsync(request.Id);
        if (journal == null || journal.UserId != request.UserId)
            return Result.Failure<JournalDto>("Journal entry not found");

        journal.Title = request.Request.Title;
        journal.Category = (JournalCategory)request.Request.Category;
        journal.SubCategory = request.Request.SubCategory;
        journal.Content = request.Request.Content;
        journal.Date = request.Request.Date;
        journal.UpdatedAt = DateTime.UtcNow;

        await _journalRepository.UpdateAsync(journal);

        var dto = MapToDto(journal);
        return Result.Success(dto, "Journal entry updated successfully");
    }

    private JournalDto MapToDto(Domain.Entities.Journal.Journal journal)
    {
        return new JournalDto
        {
            Id = journal.Id,
            Title = journal.Title,
            Category = (int)journal.Category,
            SubCategory = journal.SubCategory,
            Content = journal.Content,
            Date = journal.Date,
            CreatedAt = journal.CreatedAt,
            UpdatedAt = journal.UpdatedAt
        };
    }
}
