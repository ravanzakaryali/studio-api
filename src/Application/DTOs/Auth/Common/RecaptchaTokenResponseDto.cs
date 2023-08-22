using Newtonsoft.Json;

namespace Space.Application.DTOs;

public class RecaptchaTokenResponseDto
{
    public bool success { get; set; }
    public DateTime challenge_ts { get; set; }
    public string hostname { get; set; }
}
