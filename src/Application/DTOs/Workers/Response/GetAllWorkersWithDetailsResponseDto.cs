using System;
namespace Space.Application.DTOs;

public class GetAllWorkersWithDetailsResponseDto
{
    public Guid Id { get; set; }
    public string? EMail { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public dynamic? Roles { get; set; }
    public List<WorkersClassesDto> WorkerClasses { get; set; } = null!;
}



public class WorkersClassesDto
{
    public string ClassName { get; set; } = null!;
    public Guid ClassId { get; set; }
    public bool IsOpen { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}