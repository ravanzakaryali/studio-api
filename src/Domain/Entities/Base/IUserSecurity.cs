namespace Space.Domain.Entities;

public interface IUserSecurity
{
    public DateTime? LastPasswordUpdateDate { get; set; }
}
