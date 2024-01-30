using System;
namespace Space.Application.DTOs;



public class GetAllStudentsResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; } = null!;
    public string? Surname { get; set; } = null!;
    public string? EMail { get; set; } = null!;
    public string? Phone { get; set; } = null!;
    public List<GetAllStudentsClassDto> Classes { get; set; } = null!;
}

public class GetAllStudentsClassDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}


