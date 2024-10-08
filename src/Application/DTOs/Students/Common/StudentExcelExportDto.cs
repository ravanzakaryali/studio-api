﻿namespace Space.Application.DTOs;

public class StudentExcelExportDto
{
    public string ClassName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Email { get; set; }
    public DateOnly Date { get; set; }
    public int TotalHour { get; set; }
}
