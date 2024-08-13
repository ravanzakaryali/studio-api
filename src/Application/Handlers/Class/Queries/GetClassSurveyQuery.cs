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
            StartSurveyDate = now,
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

            ClassTimeSheet? classTimeSheet = @class.ClassTimeSheets.OrderByDescending(c => c.Date).First();

            foreach (HeldModule heldModule in classTimeSheet.HeldModules)
            {
                if (heldModule.Module?.IsSurvey == true)
                {
                    findResponse.StartSurveyDate = now;
                    continue;
                }
                else
                {
                    int? lastHeldModuleId = classTimeSheet.HeldModules.Last().ModuleId;
                    if (lastHeldModuleId == null) continue;
                    string version = modules.Where(m => m.Id == lastHeldModuleId).FirstOrDefault()?.Version ?? string.Empty;

                    modules = modules
                        .Where(m => !string.IsNullOrEmpty(m.Version) && Version.TryParse(m.Version, out _))
                        .OrderBy(m => Version.Parse(m.Version))
                        .ToList();

                    List<ClassAllSessionDto> classSessions = new();

                    ICollection<SessionDetail> sessionDetails = @class.Session.Details;

                    DateOnly startDate = @class.StartDate;
                    DateOnly? endDate = @class.EndDate;

                    DateOnly startProcessingDate = now;
                    DateOnly endProcessingDate = endDate!.Value;

                    for (DateOnly date = startProcessingDate; date <= endProcessingDate; date = date.AddDays(1))
                    {
                        DayOfWeek dayOfWeek = date.DayOfWeek;

                        IEnumerable<ClassAllSessionDto> matchingSessions = sessionDetails
                            .Where(sd => sd.DayOfWeek == dayOfWeek)
                            .Select(sd => new ClassAllSessionDto
                            {
                                Date = date,
                                TotalHours = sd.TotalHours,
                            });

                        classSessions.AddRange(matchingSessions);
                    }
                    IOrderedEnumerable<ClassAllSessionDto> classAllSessions = classSessions.Where(c => c.Date >= now).OrderByDescending(c => c.Date);


                    int moduleIndex = 0;
                    foreach (ClassAllSessionDto session in classAllSessions)
                    {
                        session.ModuleId = modules[moduleIndex].Id;
                        moduleIndex++;
                        if (modules.First(m => m.Id == session.ModuleId).IsSurvey)
                        {
                            findResponse.StartSurveyDate = session.Date;
                            break;
                        }
                    }
                }
            }
        }

        return response;
    }
}