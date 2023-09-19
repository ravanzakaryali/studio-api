namespace Space.Domain.Entities;

public class Class : BaseAuditableEntity, IKometa
{
    public Class()
    {
        ClassModulesWorkers = new HashSet<ClassModulesWorker>();
        Studies = new HashSet<Study>();
        ClassSessions = new List<ClassSession>();
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
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? VitrinDate { get; set; }
    public ICollection<Study> Studies { get; set; }
    public ICollection<ClassModulesWorker> ClassModulesWorkers { get; set; }
    public List<ClassSession> ClassSessions { get; set; }
    public ICollection<RoomSchedule> RoomSchedules { get; set; }

}
