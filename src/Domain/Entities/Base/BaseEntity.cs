namespace Space.Domain.Entities;

public class BaseEntity : IBaseEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsActive { get; set; } = true;
}
