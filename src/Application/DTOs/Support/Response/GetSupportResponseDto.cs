namespace Space.Application.DTOs;

public class GetSupportResponseDto
{
    public GetSupportResponseDto()
    {
        Images = new HashSet<GetFileResponse>();
    }
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? UserEmail { get; set; }
    public SupportStatus Status { get; set; }
    public GetClassDto? Class { get; set; }
    public ICollection<GetFileResponse> Images { get; set; }
}
