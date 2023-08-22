namespace Space.Application.Handlers;

public record GetClassesByWorkerQuery(Guid Id) : IRequest<IEnumerable<GetAllClassDto>>;

internal class GetClassesByWorkerQueryHandler : IRequestHandler<GetClassesByWorkerQuery, IEnumerable<GetAllClassDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetClassesByWorkerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetAllClassDto>> Handle(GetClassesByWorkerQuery request, CancellationToken cancellationToken)
    {
        Worker? worker = await _unitOfWork.WorkerRepository.GetAsync(request.Id, tracking: false, "ClassModulesWorkers.Class") ??
            throw new NotFoundException(nameof(Worker), request.Id);
        return worker.ClassModulesWorkers.Where(q => q.Class.EndDate > DateTime.Now).DistinctBy(cmw => cmw.ClassId).Select(cmw=>new GetAllClassDto()
        {
            Id = cmw.ClassId,
            Name = cmw.Class.Name
        });
    }
}
