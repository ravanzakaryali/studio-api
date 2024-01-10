using Newtonsoft.Json;

namespace Space.Application.DTOs;

public class RecaptchaTokenResponseDto
{
    public bool Success { get; set; }
    public DateTime Challenge_ts { get; set; }
    public string Hostname { get; set; } = null!;
}
