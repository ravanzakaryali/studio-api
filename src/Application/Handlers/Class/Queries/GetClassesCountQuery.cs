using Space.Application.DTOs.Enums;

namespace Space.Application.Handlers;

public class GetClassesCountQuery : IRequest<IEnumerable<GetClassCountResponse>>
{
}
internal class GetClassesCountQueryHandler : IRequestHandler<GetClassesCountQuery, IEnumerable<GetClassCountResponse>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassesCountQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassCountResponse>> Handle(GetClassesCountQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Class> query = _spaceDbContext.Classes.AsQueryable();
        
        int countClose = await query
            .Where(c => DateOnly.FromDateTime(DateTime.Now) > c.EndDate)
            .CountAsync(cancellationToken: cancellationToken);

        int countActive = await query
            .Where(c => DateOnly.FromDateTime(DateTime.Now) > c.StartDate && DateOnly.FromDateTime(DateTime.Now) < c.EndDate)
            .CountAsync(cancellationToken: cancellationToken);

        int countNew = await query
            .Where(c => DateOnly.FromDateTime(DateTime.Now) < c.StartDate)
            .CountAsync(cancellationToken: cancellationToken);
        return new List<GetClassCountResponse>()
        {
            new()
            {
                Count = countActive,
                Status = ClassStatus.Active
            },
            new()
            {
                Count = countClose,
                Status = ClassStatus.Close
            },
            new()
            {
                Status = ClassStatus.New,
                Count = countNew,
            }
        };
    }

}
