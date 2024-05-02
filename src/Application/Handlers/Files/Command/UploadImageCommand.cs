
using Microsoft.AspNetCore.Hosting;

namespace Space.Application.Handlers;


public class UploadImageCommand : IRequest
{
    public UploadImageCommand(IFormFile file)
    {
        File = file;
    }

    public IFormFile File { get; }
}

internal class UploadImageCommandHandler : IRequestHandler<UploadImageCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IStorageService _storageService;

    public UploadImageCommandHandler(ISpaceDbContext spaceDbContext, IStorageService storageService)
    {
        _spaceDbContext = spaceDbContext;
        _storageService = storageService;
    }

    public async Task Handle(UploadImageCommand request, CancellationToken cancellationToken)
    {
        FileUploadResponse imageUpload = await _storageService.UploadAsync(request.File, "Images");
        await _spaceDbContext.Files.AddAsync(new E.File()
        {
            FileName = imageUpload.FileName,
            Path = imageUpload.PathName,
            Extension = imageUpload.Extension,
            Storage = _storageService.StorageName,
            Size = imageUpload.Size
        });
        await _spaceDbContext.SaveChangesAsync();
    }
}