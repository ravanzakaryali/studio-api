namespace Space.Application.DTOs;

public class FileContentResponseDto
{
    public string Name { get; set; } = null!;
    public byte[] FileBytes { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}
