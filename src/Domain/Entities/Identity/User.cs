﻿namespace Space.Domain.Entities;

public class User : IdentityUser<int>, IBaseEntity
{
    public User()
    {
        UserRoles = new HashSet<UserRole>();
        Supports = new HashSet<Support>();
    }
    public override int Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Fincode { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? RefreshToken { get; set; }
    public string? ConfirmCode { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? ConfirmCodeExpires { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; }
    public ICollection<Support> Supports { get; set; }
}
