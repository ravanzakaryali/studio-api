using System;
namespace Space.Application.DTOs
{
	public class GetRoomScheduleSessionsResponseDto
	{
		public string RoomName { get; set; } = null!;
        public List<WeekScheduleSessions> WeekScheduleSessions { get; set; } = null!;

    }

	public class WeekScheduleSessions
	{
        public int WeekOfYear { get; set; }
        public IEnumerable<GetRoomScheduleSessionsByWeekDto> GetRoomScheduleSessionsByWeeks { get; set; } = null!;

    }

    public class GetRoomScheduleSessionsByWeekDto
	{
      
        public string SessionName { get; set; } = null!;
        public string ClassName { get; set; } = null!;
    }
}

