using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record CreateSupportCommand(string Title, string? Description, IFormFileCollection? Images) : IRequest
{
}
internal class CreateSupportCommandHandler : IRequestHandler<CreateSupportCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IStorageService _storageService;
    readonly ISupportRepository _supportRepository;
    readonly ICurrentUserService _currentUserService;

    public CreateSupportCommandHandler(
        IUnitOfWork unitOfWork,
        IStorageService storageService,
        ICurrentUserService currentUserService,
        ISupportRepository supportRepository)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _currentUserService = currentUserService;
        _supportRepository = supportRepository;
    }

    public async Task Handle(CreateSupportCommand request, CancellationToken cancellationToken)
    {
        string loginUserId = _currentUserService?.UserId
            ?? throw new UnauthorizedAccessException();

        User? user = await _unitOfWork.UserService.FindById(new Guid(loginUserId))
            ?? throw new NotFoundException(nameof(User), "");

        Support newSupport = new()
        {
            UserId = user.Id,
            Title = request.Title,
            Description = request.Description,
        };
        if (request.Images != null)
        {
            List<FileUploadResponse> imageUpload = await _storageService.UploadAsync(request.Images, "Images");
            newSupport.SupportImages = imageUpload.Select(i => new SupportImage()
            {
                FileName = i.FileName,
                Path = i.PathName,
                Extension = i.Extension,
                Storage = "Local",
                Size = i.Size
            }).ToList();
        }

        Support support = await _supportRepository.AddAsync(newSupport);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
