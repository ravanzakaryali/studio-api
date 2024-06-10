namespace Space.Application.DTOs;

public class CreateSupportRequestDto
{
    public CreateSupportRequestDto()
    {
        Images = new FormFileCollection();

    }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? ClassId { get; set; }
    public int CategoryId { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public IFormFileCollection? Images { get; set; }
}
