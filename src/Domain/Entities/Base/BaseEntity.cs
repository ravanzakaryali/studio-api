namespace Space.Domain.Entities;

public class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
}
