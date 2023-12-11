namespace Space.Application.DTOs;

public class GetWorkerByIdDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
}
public class GetWorkerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime? LastPasswordUpdateDate { get; set; }
    public IEnumerable<GetRoleDto>? Roles { get; set; }
}

public class GetWorkerForClassDto
{
    public int Id { get; set; }
    public string? Name { get; set; } = null!;
    public string? Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int? RoleId { get; set; }
    public string? Role { get; set; } = null!;
}

public class GetRoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
