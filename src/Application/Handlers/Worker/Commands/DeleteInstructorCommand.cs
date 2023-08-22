namespace Space.Application.Handlers;

public record DeleteWorkerCommand(Guid Id):IRequest<GetWorkerResponseDto>;
internal class DeleteWorkerCommandHandler : IRequestHandler<DeleteWorkerCommand, GetWorkerResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public DeleteWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetWorkerResponseDto> Handle(DeleteWorkerCommand request, CancellationToken cancellationToken)
    {
        Worker? Worker = await _unitOfWork.WorkerRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Worker), request.Id);
        _unitOfWork.WorkerRepository.Remove(Worker);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWorkerResponseDto>(Worker);
    }
}
