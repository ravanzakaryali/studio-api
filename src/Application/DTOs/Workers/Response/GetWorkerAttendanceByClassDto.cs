using System;
namespace Space.Application.DTOs
{
    public class GetWorkerAttendanceByClassDto
    {
        public string FullName { get; set; } = null!;

        public int? WorkerId { get; set; }

        public int Hours { get; set; }

        public string? Role { get; set; } 
    }
}

