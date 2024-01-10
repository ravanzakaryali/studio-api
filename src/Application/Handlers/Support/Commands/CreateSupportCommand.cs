using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record CreateSupportCommand(string Title, string? Description, IFormFileCollection? Images) : IRequest
{
}
internal class CreateSupportCommandHandler : IRequestHandler<CreateSupportCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IStorageService _storageService;
    readonly ICurrentUserService _currentUserService;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateSupportCommandHandler(
        IUnitOfWork unitOfWork,
        IStorageService storageService,
        ICurrentUserService currentUserService,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _storageService = storageService;
        _currentUserService = currentUserService;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateSupportCommand request, CancellationToken cancellationToken)
    {
        string loginUserId = _currentUserService?.UserId
            ?? throw new UnauthorizedAccessException();

        User? user = await _unitOfWork.UserService.FindById(int.Parse(loginUserId))
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

        await _spaceDbContext.Supports.AddAsync(newSupport);
        await _spaceDbContext.SaveChangesAsync();
    }
}
