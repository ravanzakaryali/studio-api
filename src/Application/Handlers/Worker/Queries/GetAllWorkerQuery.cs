namespace Space.Application.Handlers;

public record GetAllWorkerQuery : IRequest<IEnumerable<GetWorkerDto>>;


internal class GetAllWorkerQueryHandler : IRequestHandler<GetAllWorkerQuery, IEnumerable<GetWorkerDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetAllWorkerQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetWorkerDto>> Handle(GetAllWorkerQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<GetWorkerDto> workers = _mapper.Map<IEnumerable<GetWorkerDto>>(await _unitOfWork.WorkerRepository.GetAllAsync(predicate: null, tracking: false, "UserRoles.Role"));
        return workers.ToList().OrderBy(w => w.Name);
    }
}
