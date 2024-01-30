using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Application.DTOs
{
    internal class GetWorkerAttendanceDto
    {
        public string FullName { get; set; } = null!;
        public string? Role { get; set; }
        public string AttendanceRate { get; set; } = null!;

        public string NoAttendanceRate { get; set; } = null!;

        public string AttendancePercent { get; set; } = null!;

    }
}
