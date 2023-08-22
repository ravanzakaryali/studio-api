namespace Space.Application.DTOs;

public class GetSchedulesWorkersResponseDto
{
    public GetSchedulesWorkersResponseDto()
    {
        Schedules = new HashSet<GetSchedulesClassDto>();
    }
    public GetWorkerResponseDto Worker { get; set; } = null!;
    public ICollection<GetSchedulesClassDto> Schedules { get; set; }
}
public class GetSchedulesClassDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public GetAllClassDto Class { get; set; } = null!;
}
