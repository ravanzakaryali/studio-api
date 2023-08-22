
namespace Space.Application.DTOs;

public class GetScheduleByDayResponseDto
{
    public DateTime Date { get; set; }
    public IEnumerable<CalendarDto> Calendar { get; set; }
}


public class CalendarDto
{
    public string RoomName { get; set; } = null!;
    public int RoomCapacity { get; set; }
    public IEnumerable<SessionDtoForSchedule> Sessions { get; set; }
}

public class SessionDtoForSchedule
{
    public string ClassColor { get; set; } = null!;
    public string ClassName { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public string Endtime { get; set; } = null!;

}

