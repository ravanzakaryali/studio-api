namespace Space.Application.DTOs;

public class GetClassSessionCategoryHoursResponseDto
{
    public ClassSessionCategory Category { get; set; }
    public ClassSessionStatus? Status { get; set; }
    public int Hour { get; set; }
}
