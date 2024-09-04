namespace Space.Application.DTOs;

public class GetRoomPlaningDto
{
    public GetRoomPlaningDto()
    {
        Sessions = new HashSet<GetSessionClassResponseDto>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<GetSessionClassResponseDto> Sessions { get; set; }
}