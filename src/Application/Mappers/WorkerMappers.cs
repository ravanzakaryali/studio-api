namespace Space.Application.Mappers;

public class WorkerMappers : Profile
{
    public WorkerMappers()
    {
        CreateMap<CreateWorkerCommand, Worker>()
            .ForMember(w => w.UserName, opt => opt.MapFrom(c => c.Email));
        CreateMap<Worker, GetWorkerResponseDto>();
        CreateMap<WorkerDto, Worker>();

        CreateMap<Worker, GetWorkerForClassDto>();
        CreateMap<Worker, GetWorkerDto>()
              .ForMember(c => c.Roles, opt => opt.MapFrom(cw => cw.UserRoles.Select(ur => ur.Role)));
        CreateMap<Worker, GetWorkerByIdDto>();

        CreateMap<Worker, GetUserPermissionGroupResponseDto>()
            .ForMember(c => c.PermissionGroups, opt => opt.MapFrom(cw => cw.PermissionGroups.Select(pg => pg.Name)));
    }
}
