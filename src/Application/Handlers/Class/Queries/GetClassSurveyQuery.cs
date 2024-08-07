namespace Space.Application.Handlers;

public class GetClassSurveyQuery : IRequest<IEnumerable<GetClassSurveyDto>>
{

}
internal class GetClassSurveyQueryHandler : IRequestHandler<GetClassSurveyQuery, IEnumerable<GetClassSurveyDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassSurveyQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassSurveyDto>> Handle(GetClassSurveyQuery request, CancellationToken cancellationToken)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);

        List<Class> query = await _spaceDbContext.Classes
            .Where(c => now >= c.StartDate && now <= c.EndDate && c.ClassModulesWorkers.Count > 0)
            .ToListAsync(cancellationToken: cancellationToken);

        return query.Select(c => new GetClassSurveyDto()
        {
            Id = c.Id,
            Name = c.Name,
            EndDate = c.EndDate,
            StartDate = c.StartDate,
            Program = new GetProgramResponseDto()
            {
                Id = c.Program.Id,
                Name = c.Program.Name,
            },
        });
    }
}