using Application.Common.Result;
using Application.DTOs.Journal;
using Domain.Enums;
using Domain.Interfaces.Journal;
using MediatR;

namespace Application.Features.Journal.Commands;

public class CreateJournalCommand : IRequest<Result<JournalDto>>
{
    public Guid UserId { get; set; }
    public CreateJournalRequest Request { get; set; } = null!;
}

public class CreateJournalCommandHandler : IRequestHandler<CreateJournalCommand, Result<JournalDto>>
{
    private readonly IJournalRepository _journalRepository;

    public CreateJournalCommandHandler(IJournalRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<JournalDto>> Handle(CreateJournalCommand request, System.Threading.CancellationToken cancellationToken)
    {
        var journal = new Domain.Entities.Journal.Journal
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Request.Title,
            Category = (JournalCategory)request.Request.Category,
            SubCategory = request.Request.SubCategory,
            Content = request.Request.Content,
            Date = request.Request.Date,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _journalRepository.AddAsync(journal);

        var dto = MapToDto(journal);
        return Result.Success(dto, "Journal entry created successfully");
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
