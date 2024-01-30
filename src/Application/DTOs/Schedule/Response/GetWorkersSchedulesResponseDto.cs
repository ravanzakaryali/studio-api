using System;
namespace Space.Application.DTOs
{
	public class GetWorkersSchedulesResponseDto
	{
		public string WorkerName { get; set; } = null!;
		public List<GetWorkerClassSchedulesResponseDto> ClassSchedules { get; set; } = null!;
	}

    public class GetWorkerClassSchedulesResponseDto
	{
		public string ClassName { get; set; } = null!;
        public string ClassColor { get; set; } = null!;
		public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}

