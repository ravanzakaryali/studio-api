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
        IQueryable<Class> query = _spaceDbContext.Classes.Include(c => c.ClassSessions).AsQueryable();

        DateOnly now = DateOnly.FromDateTime(DateTime.Now);

        int countClose = await query
            .Where(c => now > c.EndDate)
            .CountAsync(cancellationToken: cancellationToken);

        int countActive = await query
            .Where(c => now >= c.StartDate && now <= c.EndDate && c.ClassSessions.Count > 0)
            .CountAsync(cancellationToken: cancellationToken);

        int countNew = await query
            .Where(c => (now > c.StartDate && c.ClassSessions.Count == 0) || (now < c.EndDate && c.ClassSessions.Count == 0) || (now <= c.StartDate && c.ClassSessions.Count == 0))
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
