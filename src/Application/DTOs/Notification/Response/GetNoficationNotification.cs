namespace Space.Application.DTOs;


public class GetNotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public UserDto? FromUser { get; set; }
    public bool IsRead { get; set; }
}