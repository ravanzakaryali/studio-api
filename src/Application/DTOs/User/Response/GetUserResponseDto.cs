using System.Net.Http.Headers;
using Microsoft.Identity.Client;

namespace Space.Application.DTOs;

public class GetUserResponseDto
{
    public GetUserResponseDto()
    {
        Permissions = new List<LoginUserPermissionDto>();
    }
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public List<string> RoleName { get; set; } = new List<string>();
    public List<LoginUserPermissionDto> Permissions { get; set; }
}
