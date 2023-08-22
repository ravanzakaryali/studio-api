namespace Space.Domain.Entities;

public class Worker : User, IKometa, IKey, IUserSecurity
{
    public Worker()
    {
        ClassModulesWorkers = new HashSet<ClassModulesWorker>();
        ClassSessions = new HashSet<ClassSession>();
    }
    public ICollection<ClassModulesWorker> ClassModulesWorkers { get; set; }
    public ICollection<ClassSession> ClassSessions { get; set; }
    public int? KometaId { get; set; }
    public Guid? Key { get; set; }
    public DateTime? KeyExpirerDate { get; set; }
    public DateTime? LastPasswordUpdateDate { get; set; }
}
