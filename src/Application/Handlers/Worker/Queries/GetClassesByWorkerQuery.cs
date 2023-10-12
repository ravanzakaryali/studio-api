using Serilog;

namespace Space.Application.Handlers;

public record GetClassesByWorkerQuery(Guid Id) : IRequest<IEnumerable<GetAllClassDto>>;

internal class GetClassesByWorkerQueryHandler : IRequestHandler<GetClassesByWorkerQuery, IEnumerable<GetAllClassDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetClassesByWorkerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllClassDto>> Handle(GetClassesByWorkerQuery request, CancellationToken cancellationToken)
    {
        Worker? worker = await _unitOfWork.WorkerRepository.GetAsync(request.Id, tracking: false, "ClassModulesWorkers.Class") ??
            throw new NotFoundException(nameof(Worker), request.Id);


        IEnumerable<ClassModulesWorker> classModuleWorker = worker.ClassModulesWorkers.Where(q => q.Class.EndDate.Value.Date >= DateTime.Now.Date).DistinctBy(cmw => cmw.ClassId);

        var classIds = classModuleWorker.Select(cm => cm.ClassId);
        var classSession = await _spaceDbContext.ClassSessions.Where(c => classIds.Contains(c.ClassId) && c.Date == DateTime.Now.Date).ToListAsync();

        return classModuleWorker.Select(cmw => new GetAllClassDto()
        {
            Id = cmw.ClassId,
            Start = classSession.Where(c => c.ClassId == cmw.ClassId)?.Min(c=>c.StartTime),
            End = classSession.Where(c => c.ClassId == cmw.ClassId)?.Max(c => c.EndTime),
            Name = cmw.Class.Name
        });
    }
}
