namespace Space.Application.DTOs;

public class Token
{
    public string AccessToken { get; set; } = null!;
    public DateTime Expires { get; set; }
}
