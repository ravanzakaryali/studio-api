namespace Space.Application.Handlers;

public record GetClassesByWorkerQuery(Guid Id) : IRequest<IEnumerable<GetAllClassDto>>;

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
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Worker), request.Id);

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        IEnumerable<ClassModulesWorker> classModuleWorker = worker.ClassModulesWorkers
            .Where(q => q.StartDate >= dateNow && q.EndDate <= dateNow)
            .DistinctBy(cmw => cmw.ClassId);

        IEnumerable<Guid> classIds = classModuleWorker.Select(cm => cm.ClassId);

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => classIds.Contains(c.ClassId) && c.Date == dateNow)
            .ToListAsync(cancellationToken: cancellationToken);


        return classModuleWorker.Select(cmw => new GetAllClassDto()
        {
            Id = cmw.ClassId,
            Start = classSessions.Where(c => c.ClassId == cmw.ClassId).Any() ?
                    classSessions.Select(c => c.StartTime).Min() :
                    null,
            End = classSessions.Where(c => c.ClassId == cmw.ClassId).Any() ?
                    classSessions.Select(c => c.StartTime).Max() :
                    null,
            Name = cmw.Class.Name
        });
    }
}
