namespace Space.Application.DTOs;


public class UpdateSupportRequestDto
{
    public SupportStatus Status { get; set; }
    public string? Note { get; set; }
}