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
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Worker), request.Id);


        IEnumerable<ClassModulesWorker> classModuleWorker = worker.ClassModulesWorkers
            .Where(q => q.StartDate >= DateTime.Now && q.EndDate <= DateTime.Now)
            .DistinctBy(cmw => cmw.ClassId);

        IEnumerable<Guid> classIds = classModuleWorker.Select(cm => cm.ClassId);
        List<ClassSession> classSession = await _spaceDbContext.ClassSessions
            .Where(c => classIds.Contains(c.ClassId) && c.Date == DateTime.Now.Date)
            .ToListAsync();

        return classModuleWorker.Select(cmw => new GetAllClassDto()
        {
            Id = cmw.ClassId,
            Start = classSession
                    .Where(c => c.ClassId == cmw.ClassId)
                    .Select(c => (TimeOnly?)c.StartTime)
                    .DefaultIfEmpty(null)
                    .Min(),
            End = classSession
                    .Where(c => c.ClassId == cmw.ClassId)
                    .Select(c => (TimeOnly?)c.EndTime)
                    .DefaultIfEmpty(null)
                    .Max(),
            Name = cmw.Class.Name
        });
    }
}
