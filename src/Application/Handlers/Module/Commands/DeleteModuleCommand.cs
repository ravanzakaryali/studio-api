namespace Space.Application.Handlers;

public record DeleteModuleCommand(Guid Id) : IRequest;

internal class DeleteModuleCommandHandler : IRequestHandler<DeleteModuleCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IModuleRepository _moduleRepository;

    public DeleteModuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {
        Module? module = await _moduleRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Module), request.Id);
        _moduleRepository.Remove(module);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

