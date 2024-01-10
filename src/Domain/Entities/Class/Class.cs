namespace Space.Domain.Entities;

public class Class : BaseAuditableEntity, IKometa
{
    public Class()
    {
        ClassModulesWorkers = new HashSet<ClassModulesWorker>();
        Studies = new HashSet<Study>();
        ClassTimeSheets = new HashSet<ClassTimeSheet>();
        ClassSessions = new HashSet<ClassSession>();
        RoomSchedules = new HashSet<RoomSchedule>();
        ClassExtraModulesWorkers = new HashSet<ClassExtraModulesWorkers>();
    }
    public string Name { get; set; } = null!;
    public bool IsNew { get; set; } = false;
    public int ProgramId { get; set; }
    public Program Program { get; set; } = null!;
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public int? RoomId { get; set; }
    public Room? Room { get; set; }
    public int? KometaId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ICollection<Study> Studies { get; set; }
    public ICollection<ClassModulesWorker> ClassModulesWorkers { get; set; }
    public ICollection<ClassTimeSheet> ClassTimeSheets { get; set; }
    public ICollection<ClassSession> ClassSessions { get; set; }
    public ICollection<RoomSchedule> RoomSchedules { get; set; }
    public ICollection<ClassExtraModulesWorkers> ClassExtraModulesWorkers { get; set; }

}
