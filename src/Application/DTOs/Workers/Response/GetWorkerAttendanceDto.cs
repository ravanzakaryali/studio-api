using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Application.DTOs
{
    internal class GetWorkerAttendanceDto
    {
        public string FullName { get; set; }
        public string Role { get; set; }
        public string AttendanceRate { get; set; }

        public string NoAttendanceRate { get; set; }

        public string AttendancePercent { get; set; }

    }
}
