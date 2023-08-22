namespace Space.Application.Handlers;

public record DeleteSupportCommand(Guid Id) : IRequest;

internal class DeleteSupportCommandHandler : IRequestHandler<DeleteSupportCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IStorageService _storageService;

    public DeleteSupportCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IStorageService storageService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _storageService = storageService;
    }

    public async Task Handle(DeleteSupportCommand request, CancellationToken cancellationToken)
    {
        Support? support = await _unitOfWork.SupportRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Support), request.Id);
        foreach (SupportImage item in support.SupportImages)
        {
            _storageService.Delete(item.FileName);
        }
        //Todo: Hard delete yaptım
        _unitOfWork.SupportRepository.Remove(support, true);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
