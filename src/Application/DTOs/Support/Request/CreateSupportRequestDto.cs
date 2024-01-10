namespace Space.Application.DTOs;

public class CreateSupportRequestDto
{
    public CreateSupportRequestDto()
    {
        Images = new FormFileCollection();
    }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public IFormFileCollection? Images { get; set; }
}
