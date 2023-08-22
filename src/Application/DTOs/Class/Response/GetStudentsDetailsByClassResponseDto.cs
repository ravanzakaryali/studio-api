namespace Space.Application.DTOs;

internal class GetStudentsDetailsByClassResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string FatherName { get; set; } = null!;
    public string ClassName { get; set; } = null!;
    public double ArrivalHours { get; set; }
    public double AbsentHours { get; set; }
    public double Attendance { get; set; }
    public Guid? StudentId { get; set; }
}
