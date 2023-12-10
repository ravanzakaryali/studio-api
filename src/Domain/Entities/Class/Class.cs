namespace Space.Domain.Entities;

public class Class : BaseAuditableEntity, IKometa
{
    public Class()
    {
        ClassModulesWorkers = new HashSet<ClassModulesWorker>();
        Studies = new HashSet<Study>();
        ClassTimeSheets = new HashSet<ClassTimeSheet>();
        ClassGenerateSessions = new HashSet<ClassGenerateSession>();
        RoomSchedules = new HashSet<RoomSchedule>();
    }
    public string Name { get; set; } = null!;
    public bool IsNew { get; set; } = false;
    public Guid ProgramId { get; set; }
    public Program Program { get; set; } = null!;
    public Guid SessionId { get; set; }
    public Session Session { get; set; } = null!;
    public Guid? RoomId { get; set; }
    public Room? Room { get; set; }
    public int? KometaId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public ICollection<Study> Studies { get; set; }
    public ICollection<ClassModulesWorker> ClassModulesWorkers { get; set; }
    public ICollection<ClassTimeSheet> ClassTimeSheets { get; set; }
    public ICollection<ClassGenerateSession> ClassGenerateSessions { get; set; }
    public ICollection<RoomSchedule> RoomSchedules { get; set; }

}
