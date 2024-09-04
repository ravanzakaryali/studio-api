namespace Space.Application.DTOs;

public class GetSessionResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

}
public class GetSessionClassResponseDto : GetSessionResponseDto
{
    public GetSessionClassResponseDto()
    {
        Classes = new List<GetClassDetailDto>();
    }
    public List<GetClassDetailDto> Classes { get; set; }
}
