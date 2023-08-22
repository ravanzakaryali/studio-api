namespace Space.Application.Handlers;

public record GetWorkerQuery(Guid Id) : IRequest<GetWorkerByIdDto>;
public class GetWorkerQueryCommand : IRequestHandler<GetWorkerQuery, GetWorkerByIdDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetWorkerQueryCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetWorkerByIdDto> Handle(GetWorkerQuery request, CancellationToken cancellationToken)
    {
        Worker? worker = await _unitOfWork.WorkerRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Worker), request.Id);
        return _mapper.Map<GetWorkerByIdDto>(worker);
    }
}
