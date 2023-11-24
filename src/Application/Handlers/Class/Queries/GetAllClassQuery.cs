namespace Space.Application.Handlers.Queries;

public record GetAllClassQuery(ClassStatus Status) : IRequest<IEnumerable<GetClassModuleWorkersResponse>>;
internal class GetAllClassHandler : IRequestHandler<GetAllClassQuery, IEnumerable<GetClassModuleWorkersResponse>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllClassHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassModuleWorkersResponse>> Handle(GetAllClassQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Class> query = _spaceDbContext.Classes
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .Include(c => c.ClassSessions)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Worker)
            .ThenInclude(c => c.UserRoles)
            .ThenInclude(c => c.Role)
            .Include(c => c.Session).AsQueryable();

        if (request.Status == ClassStatus.Close)
        {
            query = query.Where(c => DateTime.Now > c.EndDate);
        }
        else if (request.Status == ClassStatus.Active)
        {
            query = query.Where(c => DateTime.Now > c.StartDate && DateTime.Now < c.EndDate);
        }
        else
        {
            query = query.Where(c => DateTime.Now < c.StartDate);
        }

        List<GetClassModuleWorkers> classes = await query.Select(cd => new GetClassModuleWorkers()
        {
            Id = cd.Id,
            TotalHour = cd.ClassSessions
                            .Where(c =>
                            c.Status != ClassSessionStatus.Cancelled &&
                            c.Category != ClassSessionCategory.Lab)
                        .Sum(c => c.TotalHours),
            CurrentHour = cd.ClassSessions
                            .Where(c => c.Status != ClassSessionStatus.Cancelled &&
                                        c.Category != ClassSessionCategory.Lab &&
                                        c.Date <= DateOnly.FromDateTime(DateTime.UtcNow))
                            .Sum(c => c.TotalHours),
            ClassName = cd.Name,
            EndDate = cd.EndDate,
            ProgramId = cd.ProgramId,
            ProgramName = cd.Program.Name,
            SessionName = cd.Session.Name,
            StartDate = cd.StartDate,
            TotalModules = cd.Program.Modules.Count,
            VitrinDate = cd.StartDate,
            ClassModulesWorkers = cd.ClassModulesWorkers
        }).ToListAsync(cancellationToken: cancellationToken);

        return classes.Select(cd => new GetClassModuleWorkersResponse()
        {
            Id = cd.Id,
            TotalHour = cd.TotalHour,
            ClassName = cd.ClassName,
            CurrentHour = cd.CurrentHour,
            EndDate = cd.EndDate,
            ProgramId = cd.ProgramId,
            ProgramName = cd.ProgramName,
            SessionName = cd.SessionName,
            StartDate = cd.StartDate,
            TotalModules = cd.TotalModules,
            VitrinDate = cd.VitrinDate,
            Workers = cd.ClassModulesWorkers.Select(cmw => new GetWorkerForClassDto()
            {
                Id = cmw.Worker.Id,
                Email = cmw.Worker.Email,
                Name = cmw.Worker.Name,
                Surname = cmw.Worker.Surname,
                Role = cmw.Worker.UserRoles.FirstOrDefault(u => u.RoleId == cmw.RoleId)?.Role.Name,
                RoleId = cmw.Worker.UserRoles.FirstOrDefault(u => u.RoleId == cmw.RoleId)?.Role.Id
            }).Distinct(new GetModulesWorkerComparer())
        });
    }
}

