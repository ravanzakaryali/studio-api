namespace Space.Application.Mappers;

public class RoleMappers : Profile
{
    public RoleMappers()
    {
        CreateMap<Role, GetRoleDto>();
    }
}
