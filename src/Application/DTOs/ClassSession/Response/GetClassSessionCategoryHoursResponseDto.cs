namespace Space.Application.DTOs;

public class GetClassSessionCategoryHoursResponseDto
{
    public string CategoryName { get; set; } = null!;
    public ClassSessionStatus? Status { get; set; }
    public int Hour { get; set; }
}
