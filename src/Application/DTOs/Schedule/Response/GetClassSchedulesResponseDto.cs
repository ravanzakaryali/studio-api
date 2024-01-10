using System;
namespace Space.Application.DTOs
{
	public class GetClassSchedulesResponseDto
	{
		public string ClassName { get; set; } = null!;
		public int ClassId { get; set; } 
        public string RoomName { get; set; } = null!;
        public string ClassColor { get; set; } = null!;
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

	}
}

