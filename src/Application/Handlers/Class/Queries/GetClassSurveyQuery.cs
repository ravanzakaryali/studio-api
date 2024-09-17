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

        List<Class> classes = await _spaceDbContext.Classes

            .Where(c => now >= c.StartDate && now <= c.EndDate && c.ClassModulesWorkers.Count > 0)
            .Include(c => c.Session)
            .ThenInclude(c => c.Details)
            .Include(c => c.Program)
            .Include(c => c.ClassTimeSheets)
            .ThenInclude(c => c.HeldModules)
            .ToListAsync(cancellationToken: cancellationToken);

        List<Module> modules = await _spaceDbContext.Modules
            .Where(m => m.TopModuleId != null)
            .ToListAsync(cancellationToken: cancellationToken);

        modules = modules.OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null).ToList();

        IEnumerable<GetClassSurveyDto> response = classes.Select(c => new GetClassSurveyDto()
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

        foreach (Class @class in classes)
        {
            GetClassSurveyDto? findResponse = response.Where(c => c.Id == @class.Id).FirstOrDefault();
            if (findResponse == null) continue;

            if (@class == null) continue;

            // ClassTimeSheet? classTimeSheet = @class.ClassTimeSheets.OrderByDescending(c => c.Date).FirstOrDefault();
            // if (classTimeSheet == null) continue;

            foreach (var classTimeSheet in @class.ClassTimeSheets)
            {

                foreach (HeldModule heldModule in classTimeSheet.HeldModules)
                {
                    if (heldModule.Module?.IsSurvey == true)
                    {
                        findResponse.StartSurveyDate = classTimeSheet.Date;
                    }
                }
            }

        }

        return response;
    }
}