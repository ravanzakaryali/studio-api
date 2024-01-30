namespace Space.Application.DTOs;

public class CreateStudentsDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? FatherName { get; set; }
}
