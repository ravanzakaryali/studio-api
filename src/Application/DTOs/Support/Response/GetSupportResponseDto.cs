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
    public GetSupportUser User { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public SupportStatus Status { get; set; }
    public GetClassDto? Class { get; set; }
    public GetSupportCategory? SupportCategory { get; set; }
    public ICollection<GetFileResponse> Images { get; set; }
}

public class GetSupportUser
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}


