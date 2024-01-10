namespace Space.Application.DTOs;

internal class GetStudentsDetailsByClassResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? FatherName { get; set; }
    public string ClassName { get; set; } = null!;
    public double? ArrivalHours { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public double AbsentHours { get; set; }
    public double Attendance { get; set; }
    public int? StudentId { get; set; }
}
