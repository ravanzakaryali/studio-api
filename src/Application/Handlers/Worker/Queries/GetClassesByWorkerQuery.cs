namespace Space.Application.Handlers;

public record GetClassesByWorkerQuery() : IRequest<IEnumerable<GetAllClassDto>>;

internal class GetClassesByWorkerQueryHandler : IRequestHandler<GetClassesByWorkerQuery, IEnumerable<GetAllClassDto>>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;
    readonly ICurrentUserService _currentUserService;

    public GetClassesByWorkerQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext,
        ICurrentUserService currentUserService)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<GetAllClassDto>> Handle(GetClassesByWorkerQuery request, CancellationToken cancellationToken)
    {

        string? loginUserId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        int userId = int.Parse(loginUserId);
        Worker? worker = await _spaceDbContext.Workers
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Class)
            .ThenInclude(c => c.Session)
            .ThenInclude(c => c.Details)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Class)
            .ThenInclude(c => c.Program)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Module)
            .Where(c => c.Id == userId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Worker), userId);

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(4));

        IEnumerable<ClassModulesWorker> classModuleWorker = worker.ClassModulesWorkers
            .Where(q => q.StartDate <= dateNow && q.EndDate >= dateNow)
            .Where(c => c.Class.StartDate <= dateNow && c.Class.EndDate >= dateNow)
            .DistinctBy(cmw => cmw.ClassId);

        IEnumerable<int> classIds = classModuleWorker.Select(cm => cm.ClassId);

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => classIds.Contains(c.ClassId) && c.Status != ClassSessionStatus.Cancelled)
            .ToListAsync(cancellationToken: cancellationToken);


        return classModuleWorker
        .Select(cmw => new GetAllClassDto()
        {
            Id = cmw.ClassId,
            Start = cmw.Class.Session.Details.Select(c => c.StartTime).Min(),
            ProgramId = cmw.Class.ProgramId,
            End = cmw.Class.Session.Details.Select(c => c.EndTime).Max(),
            AttendanceHours = classTimeSheets.Where(c => c.ClassId == cmw.ClassId).Sum(c => c.TotalHours),
            TotalHours = cmw.Class.Program.TotalHours,
            Name = cmw.Class.Name
        });
    }
}
