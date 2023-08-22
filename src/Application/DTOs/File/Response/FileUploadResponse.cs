namespace Space.Application.DTOs;

public class FileUploadResponse
{
    public string FileName { get; set; } = null!;
    public string PathName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public string Extension { get; set; } = null!;
    public double Size { get; set; }
}
