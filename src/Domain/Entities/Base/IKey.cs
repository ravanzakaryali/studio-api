namespace Space.Domain.Entities;

public interface IKey
{
    public Guid? Key { get; set; }
    public DateTime? KeyExpirerDate { get; set; }
}
