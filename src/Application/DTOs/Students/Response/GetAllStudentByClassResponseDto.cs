namespace Space.Application.DTOs;

public class GetAllStudentByClassResponseDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string ClassName { get; set; } = null!;
    public string? Name { get; set; } = null!;
    public string? Surname { get; set; } = null!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public double? Attendance { get; set; }
    public IEnumerable<GetAllStudentCategoryDto> Sessions { get; set; } = null!;

}

public class GetAllStudentCategoryDto
{
    public ClassSessionCategory? ClassSessionCategory { get; set; }
    public int? Hour { get; set; }
    public string? Note { get; set; }
}