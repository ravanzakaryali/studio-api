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
        List<Class> query = await _spaceDbContext.Classes.Include(c => c.ClassModulesWorkers).ToListAsync();

        DateOnly now = DateOnly.FromDateTime(DateTime.Now);

        int countClose = query
            .Where(c => now > c.EndDate)
            .Count();

        int countActive = query
            .Where(c => now >= c.StartDate && now <= c.EndDate && c.ClassModulesWorkers.Count > 0)
            .Count();

        int countNew = query
            .Where(c => (now > c.StartDate && c.ClassModulesWorkers.Count <= 0) || (now < c.StartDate && c.ClassExtraModulesWorkers.Count <= 0))
            .Count();

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
