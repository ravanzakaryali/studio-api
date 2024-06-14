namespace Space.Application.Mappers;

public class PermissionGroupMappers : Profile
{
    public PermissionGroupMappers()
    {
        CreateMap<PermissionGroup, GetPermissionGroupDto>()
        .ForMember(pg => pg.UserCount, opt => opt.MapFrom(pg => pg.Workers.Count));

        CreateMap<PermissionGroup, PermissionGroupDto>();
    }
}