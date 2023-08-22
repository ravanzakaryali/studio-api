using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Mime;

namespace Space.Application.Handlers;
public record GetImageQuery(string ImageName) : IRequest<FileContentResponseDto>;

internal class GetImageQueryHandler : IRequestHandler<GetImageQuery, FileContentResponseDto>
{
    readonly IWebHostEnvironment _webHostEnvironment;
    readonly IUnitOfWork _unitOfWork;

    public GetImageQueryHandler(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
    {
        _webHostEnvironment = webHostEnvironment;
        _unitOfWork = unitOfWork;
    }

    public async Task<FileContentResponseDto> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        E.File file = await _unitOfWork.FileRepository.GetAsync(f => f.FileName == request.ImageName)
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
