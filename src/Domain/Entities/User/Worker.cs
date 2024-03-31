namespace Space.Domain.Entities;

public class Worker : User, IKometa, IKey, IUserSecurity
{
    public Worker()
    {
        ClassModulesWorkers = new HashSet<ClassModulesWorker>();
        Reservations = new HashSet<Reservation>();
        AttendancesWorkers = new HashSet<AttendanceWorker>();
        PermissionGroupWorkerPermissionLevels = new HashSet<PermissionGroupWorkerPermissionLevel>();
    }
    public int? KometaId { get; set; }
    public Guid? Key { get; set; }
    public DateTime? KeyExpirerDate { get; set; }
    public DateTime? LastPasswordUpdateDate { get; set; }
    public ICollection<ClassModulesWorker> ClassModulesWorkers { get; set; }
    public ICollection<AttendanceWorker> AttendancesWorkers { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    public ICollection<PermissionGroupWorkerPermissionLevel> PermissionGroupWorkerPermissionLevels { get; set; }
}
