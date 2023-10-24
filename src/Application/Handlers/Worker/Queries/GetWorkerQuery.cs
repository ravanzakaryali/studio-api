namespace Space.Application.Handlers;

public record GetWorkerQuery(Guid Id) : IRequest<GetWorkerByIdDto>;
public class GetWorkerQueryCommand : IRequestHandler<GetWorkerQuery, GetWorkerByIdDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IWorkerRepository _workerRepository;

    public GetWorkerQueryCommand(IUnitOfWork unitOfWork, IMapper mapper, IWorkerRepository workerRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _workerRepository = workerRepository;
    }

    public async Task<GetWorkerByIdDto> Handle(GetWorkerQuery request, CancellationToken cancellationToken)
    {
        Worker? worker = await _workerRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Worker), request.Id);
        return _mapper.Map<GetWorkerByIdDto>(worker);
    }
}
