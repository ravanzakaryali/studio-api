namespace Space.Application.DTOs;

public class GetWorkerByIdDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
}
public class GetWorkerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime? LastPasswordUpdateDate { get; set; }
    public IEnumerable<GetRoleDto>? Roles { get; set; }
}

public class GetWorkerForClassDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid RoleId { get; set; }
    public string Role { get; set; } = null!;
}

public class GetRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
