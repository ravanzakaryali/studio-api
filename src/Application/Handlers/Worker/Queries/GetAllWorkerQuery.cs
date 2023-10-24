namespace Space.Application.Handlers;

public record GetAllWorkerQuery : IRequest<IEnumerable<GetWorkerDto>>;


internal class GetAllWorkerQueryHandler : IRequestHandler<GetAllWorkerQuery, IEnumerable<GetWorkerDto>>
{
    readonly IWorkerRepository _workerRepository;
    readonly IMapper _mapper;

    public GetAllWorkerQueryHandler(IMapper mapper, IWorkerRepository workerRepository)
    {
        _mapper = mapper;
        _workerRepository = workerRepository;
    }

    public async Task<IEnumerable<GetWorkerDto>> Handle(GetAllWorkerQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<GetWorkerDto> workers = _mapper.Map<IEnumerable<GetWorkerDto>>(await _workerRepository.GetAllAsync(predicate: null, tracking: false, "UserRoles.Role"));
        return workers.ToList().OrderBy(w => w.Name);
    }
}
