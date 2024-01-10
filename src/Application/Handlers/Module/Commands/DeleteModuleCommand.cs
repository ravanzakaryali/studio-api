namespace Space.Application.Handlers;

public record DeleteModuleCommand(int Id) : IRequest;

internal class DeleteModuleCommandHandler : IRequestHandler<DeleteModuleCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public DeleteModuleCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(DeleteModuleCommand request, CancellationToken cancellationToken)
    {
        Module? module = await _spaceDbContext.Modules.FindAsync(request.Id)
            ?? throw new NotFoundException(nameof(Module), request.Id);
        module.IsDeleted = true;
        await _spaceDbContext.SaveChangesAsync();
    }
}

