namespace Space.Domain.Entities;

public class Room : BaseAuditableEntity, IKometa
{
    public Room()
    {
        Classes = new HashSet<Class>();
        RoomSchedules = new HashSet<RoomSchedule>();
    }
    public string Name { get; set; } = null!;
    public int? Capacity { get; set; }
    public RoomType Type { get; set; }
    public int? KometaId { get; set; }
    public ICollection<Class> Classes { get; set; }
    public ICollection<RoomSchedule> RoomSchedules { get; set; }

}
