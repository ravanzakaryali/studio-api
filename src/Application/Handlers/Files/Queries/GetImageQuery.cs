using Microsoft.AspNetCore.StaticFiles;
using System.Net.Mime;

namespace Space.Application.Handlers;
public record GetImageQuery(string ImageName) : IRequest<FileContentResponseDto>;

internal class GetImageQueryHandler : IRequestHandler<GetImageQuery, FileContentResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetImageQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<FileContentResponseDto> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        E.File file = await _spaceDbContext.Files.Where(f => f.FileName == request.ImageName).FirstOrDefaultAsync()
            ?? throw new NotFoundException("Image not found");
        if (!IO.File.Exists(file.Path)) throw new NotFoundException("File not found");
        FileExtensionContentTypeProvider provider = new();

        byte[] imageBytes = IO.File.ReadAllBytes(file.Path);
        if (!provider.TryGetContentType(file.Path, out string? contentType))
        {
            contentType = GetMimeType(file.Extension);
        }
        return new FileContentResponseDto()
        {
            ContentType = contentType,
            FileBytes = imageBytes
        };
    }
    string GetMimeType(string extension)
    {
        string mimeType = MediaTypeNames.Application.Octet;

        if (extension == ".jpg" || extension == ".jpeg" || extension == ".jpe")
        {
            mimeType = MediaTypeNames.Image.Jpeg;
        }
        else if (extension == ".png")
        {
            mimeType = "image/png";
        }
        else if (extension == ".gif")
        {
            mimeType = MediaTypeNames.Image.Gif;
        }
        else if (extension == ".bmp")
        {
            mimeType = "image/bmp";
        }
        else if (extension == ".tiff")
        {
            mimeType = MediaTypeNames.Image.Tiff;
        }
        else if (extension == ".svg")
        {
            mimeType = "image/svg+xml";
        }

        return mimeType;
    }
}
