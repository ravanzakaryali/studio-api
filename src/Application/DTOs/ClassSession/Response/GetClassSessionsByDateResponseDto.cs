using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Application.DTOs
{
    public class GetClassSessionsByDateResponseDto
    {
        public string ClassName { get; set; } = null!;
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public int TotalHour { get; set; }
        public ClassSessionStatus? Status { get; set; }

    }
}
