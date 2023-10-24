using System;
namespace Space.Application.DTOs;


    public class GetWorkerClassSessionsByClassResponseDto
    {
    public Guid ClassId { get; set; }
    public string ClassName { get; set; } = null!;
    public int OfflineHours { get; set; }
    public int OnlineHours { get; set; }
    public int CancaledHours { get; set; }

    public List<GetWorkerClassSessionsDto> WorkerClassSessions { get; set; } = null!;

}


public class GetWorkerClassSessionsDto
{
    public DateTime Date { get; set; }
    public string? Category { get; set; }
    public string? Status { get; set; }
    public int TotalHours { get; set; }


}


