namespace Space.Application.Handlers;

public record DeleteSupportCommand(int Id) : IRequest;

internal class DeleteSupportCommandHandler : IRequestHandler<DeleteSupportCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IStorageService _storageService;
    readonly ISpaceDbContext _spaceDbContext;

    public DeleteSupportCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStorageService storageService,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _storageService = storageService;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(DeleteSupportCommand request, CancellationToken cancellationToken)
    {
        Support? support = await _spaceDbContext.Supports.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Support), request.Id);
        foreach (SupportImage item in support.SupportImages)
        {
            _storageService.Delete(item.FileName);
        }
        //Todo: Hard delete yaptım
        _spaceDbContext.Supports.Remove(support);
        await _spaceDbContext.SaveChangesAsync();
    }
}
