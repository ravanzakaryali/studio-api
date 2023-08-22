namespace Space.Application.DTOs;

public class FileContentResponseDto
{
    public byte[] FileBytes { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}
