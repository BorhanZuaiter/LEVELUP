using Application.Common.Result;
using Application.DTOs.Journal;
using Domain.Interfaces.Journal;
using MediatR;

namespace Application.Features.Journal.Queries;

public class GetByDateQuery : IRequest<Result<List<JournalDto>>>
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
}

public class GetByDateQueryHandler : IRequestHandler<GetByDateQuery, Result<List<JournalDto>>>
{
    private readonly IJournalRepository _journalRepository;

    public GetByDateQueryHandler(IJournalRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    public async global::System.Threading.Tasks.Task<Result<List<JournalDto>>> Handle(GetByDateQuery request, System.Threading.CancellationToken cancellationToken)
    {
        var journals = await _journalRepository.GetByDateAsync(request.UserId, request.Date);
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
