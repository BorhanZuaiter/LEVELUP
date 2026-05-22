using Application.Common.Result;
using Application.DTOs.Journal;
using Domain.Interfaces.Journal;
using MediatR;

namespace Application.Features.Journal.Queries;

public class GetTimelineQuery : IRequest<Result<List<JournalDto>>>
{
    public Guid UserId { get; set; }
}

public class GetTimelineQueryHandler : IRequestHandler<GetTimelineQuery, Result<List<JournalDto>>>
{
    private readonly IJournalRepository _journalRepository;

    public GetTimelineQueryHandler(IJournalRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<List<JournalDto>>> Handle(GetTimelineQuery request, System.Threading.CancellationToken cancellationToken)
    {
        var journals = await _journalRepository.GetTimelineAsync(request.UserId);
        var dtos = journals.Select(MapToDto).ToList();
        return Result.Success(dtos);
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
