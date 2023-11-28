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


        IEnumerable<ClassModulesWorker> classModuleWorker = worker.ClassModulesWorkers
            .Where(q => q.StartDate >= DateOnly.FromDateTime(DateTime.Now) && q.EndDate <= DateOnly.FromDateTime(DateTime.Now))
            .DistinctBy(cmw => cmw.ClassId);

        IEnumerable<Guid> classIds = classModuleWorker.Select(cm => cm.ClassId);
        List<ClassTimeSheet> classTimeSheet = await _spaceDbContext.ClassTimeSheets
            .Where(c => classIds.Contains(c.ClassId) && c.Date == DateOnly.FromDateTime(DateTime.Now.Date))
            .ToListAsync();

        return classModuleWorker.Select(cmw => new GetAllClassDto()
        {
            Id = cmw.ClassId,
            Start = classTimeSheet
                    .Where(c => c.ClassId == cmw.ClassId)
                    .Select(c => c.StartTime)
                    .Min(),
            End = classTimeSheet
                    .Where(c => c.ClassId == cmw.ClassId)
                    .Select(c => c.EndTime)
                    .Max(),
            Name = cmw.Class.Name
        });
    }
}
