namespace Space.Application.DTOs;

public class StudentExcelExportDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime Date { get; set; }
    public int TotalHour { get; set; }
}
