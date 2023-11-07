namespace Space.Application.DTOs;

public class GetAllAbsentStudentResponseDto
{
    public Guid Id { get; set; }
    public Guid? StudentId { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Father { get; set; }
    public int AbsentCount { get; set; }
    public GetAllClassDto Class { get; set; } = null!;
}
