namespace Space.Application.DTOs;

public class GetWithIncludeClassResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public GetProgramResponseDto Program { get; set; } = null!;
    public GetRoomResponseDto? Room { get; set; }
    public GetSessionWithDetailsResponseDto Session { get; set; } = null!;
}
