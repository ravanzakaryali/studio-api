using System;
namespace Space.Application.DTOs
{
	public class GetClassScheduleSessionsByWeekResponseDto
	{
		public string ClassName { get; set; } = null!;
		public int StartWeek { get; set; }
        public int EndWeek { get; set; }
		public string RoomName { get; set; } = null!;	

    }
}

