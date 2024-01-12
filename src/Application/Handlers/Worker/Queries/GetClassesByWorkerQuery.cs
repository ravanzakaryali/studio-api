namespace Space.Application.Handlers;

public record GetClassesByWorkerQuery(int Id) : IRequest<IEnumerable<GetAllClassDto>>;

internal class GetClassesByWorkerQueryHandler : IRequestHandler<GetClassesByWorkerQuery, IEnumerable<GetAllClassDto>>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassesByWorkerQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllClassDto>> Handle(GetClassesByWorkerQuery request, CancellationToken cancellationToken)
    {
        Worker? worker = await _spaceDbContext.Workers
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Class)
            .Include(c=>c.ClassModulesWorkers)
            .ThenInclude(c=>c.Module)
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Worker), request.Id);

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(4));

        IEnumerable<ClassModulesWorker> classModuleWorker = worker.ClassModulesWorkers
            .Where(q => q.StartDate <= dateNow && q.EndDate >= dateNow)
            .DistinctBy(cmw => cmw.ClassId);

        IEnumerable<int> classIds = classModuleWorker.Select(cm => cm.ClassId);

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => classIds.Contains(c.ClassId))
            .ToListAsync(cancellationToken: cancellationToken);

        return classModuleWorker
        .Select(cmw => new GetAllClassDto()
        {
            Id = cmw.ClassId,
            Start = classSessions.Where(c => c.ClassId == cmw.ClassId && c.Date == dateNow).Any() ?
                    classSessions.Select(c => c.StartTime).Min() :
                    null,
            ProgramId = cmw.Class.ProgramId,
            End = classSessions.Where(c => c.ClassId == cmw.ClassId && c.Date == dateNow).Any() ?
                    classSessions.Select(c => c.StartTime).Max() :
                    null,
            AttendanceHours = classSessions
                            .Where(c => c.ClassTimeSheetId != null && c.Status != ClassSessionStatus.Cancelled && c.ClassId == cmw.ClassId && c.Category != ClassSessionCategory.Practice)
                            .Sum(c => c.TotalHours),
            TotalHours = classSessions
                            .Where(c => c.Status != ClassSessionStatus.Cancelled && c.ClassId == cmw.ClassId && c.Category != ClassSessionCategory.Practice)
                            .Sum(c => c.TotalHours),
            Name = cmw.Class.Name
        });
    }
}
