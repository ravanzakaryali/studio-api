namespace Space.Application.Handlers;

public record DeleteWorkerCommand(Guid Id) : IRequest<GetWorkerResponseDto>;
internal class DeleteWorkerCommandHandler : IRequestHandler<DeleteWorkerCommand, GetWorkerResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IWorkerRepository _workerRepository;

    public DeleteWorkerCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IWorkerRepository workerRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _workerRepository = workerRepository;
    }

    public async Task<GetWorkerResponseDto> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
    {
        Worker? Worker = await _workerRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Worker), request.Id);
        _workerRepository.Remove(Worker);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWorkerResponseDto>(Worker);
    }
}
