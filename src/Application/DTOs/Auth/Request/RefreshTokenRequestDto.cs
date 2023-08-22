namespace Space.Application.DTOs;

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = null!;
    public string AccessToken { get; set;}= null!;
}
