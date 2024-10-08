﻿namespace Space.Domain.Entities;

public class ClassTimeSheet : BaseAuditableEntity, ICloneable
{
    public ClassTimeSheet()
    {
        Attendances = new HashSet<Attendance>();
        AttendancesWorkers = new List<AttendanceWorker>();
        HeldModules = new HashSet<HeldModule>();
    }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int TotalHours { get; set; }
    public ClassSessionStatus Status { get; set; }
    public string? Note { get; set; }
    public ClassSessionCategory Category { get; set; }
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;
    public ClassSession ClassSession { get; set; } = null!;
    public ICollection<HeldModule> HeldModules { get; set; }
    public List<AttendanceWorker> AttendancesWorkers { get; set; }
    public ICollection<Attendance> Attendances { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
