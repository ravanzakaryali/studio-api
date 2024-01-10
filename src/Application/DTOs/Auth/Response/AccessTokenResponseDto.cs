namespace Space.Application.DTOs;

public class AccessTokenResponseDto
{
    //public int UserId { get; set; }
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    //public DateTime TokenExpires { get; set; }
}
