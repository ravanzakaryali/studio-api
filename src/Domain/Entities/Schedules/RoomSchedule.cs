using System;
using Space.Domain.Entities;

namespace Space.Domain.Entities
{
    public class RoomSchedule : BaseAuditableEntity
    {
        public DateTime GeneralDate { get; set; }
        public int DayOfWeek { get; set; }
        public int DayOfMonth { get; set; }
        public int Year { get; set; }
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public int? RoomId { get; set; }
        public Room? Room { get; set; }
        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        public EnumScheduleCategory Category { get; set; }
        public int? ClassId { get; set; }
        public Class? @Class { get; set; }

    }
}

