using System;
namespace Space.Application.DTOs;



public class GetAllStudentsResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string EMail { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public List<GetAllStudentsClassDto> Classes { get; set; }
}

public class GetAllStudentsClassDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}


