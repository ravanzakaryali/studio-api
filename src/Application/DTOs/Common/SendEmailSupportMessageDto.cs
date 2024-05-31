namespace Space.Application.DTOs;
public class SendEmailSupportMessageDto
{
    public SendEmailSupportMessageDto()
    {
        To = new HashSet<string>();
    }
    public string Title { get; set; } = null!;
    public IEnumerable<string> To { get; set; }
    public string? Message { get; set; }
    public UserDto User { get; set; } = null!;
}