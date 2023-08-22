using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space.Application.DTOs
{
    public class GetAllClassSessionByClassResponseDto
    {
        public string ClassName { get; set; }
        public DateTime ClassSessionDate { get; set; }
        public ClassSessionStatus? ClassSessionStatus { get; set; }
        public Guid ClassId { get; set; }
    }
}
