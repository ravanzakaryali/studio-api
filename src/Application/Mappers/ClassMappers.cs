using Space.Application.DTOs;

namespace Space.Application.Mappers;

public class ClassMappers : Profile
{
    public ClassMappers()
    {
        CreateMap<CreateClassCommand, Class>();
        CreateMap<Class, GetClassResponseDto>();
        CreateMap<UpdateClassCommand, Class>();
        CreateMap<ClassModulesWorker, GetWorkerDto>()
           .ForMember(c => c.Id, opt => opt.MapFrom(cw => cw.WorkerId))
           .ForMember(c => c.Name, opt => opt.MapFrom(cw => cw.Worker.Name))
           .ForMember(c => c.Surname, opt => opt.MapFrom(cw => cw.Worker.Surname))
           .ForMember(c => c.Email, opt => opt.MapFrom(cw => cw.Worker.Email))
           .ForMember(c => c.Roles, opt => opt.MapFrom(cw => cw.Worker.UserRoles.Select(ur => ur.Role)));

        CreateMap<ClassModulesWorker, GetWorkerForClassDto>()
           .ForMember(c => c.Id, opt => opt.MapFrom(cw => cw.WorkerId))
           .ForMember(c => c.Name, opt => opt.MapFrom(cw => cw.Worker.Name))
           .ForMember(c => c.Surname, opt => opt.MapFrom(cw => cw.Worker.Surname))
           .ForMember(c => c.Email, opt => opt.MapFrom(cw => cw.Worker.Email))
           .ForMember(c => c.Role, opt => opt.MapFrom(cw => cw.Role.Name))
           .ForMember(c => c.RoleId, opt => opt.MapFrom(cw => cw.Role.Id));
        CreateMap<Class, GetClassModuleWorkers>()
            .ForMember(c => c.Id, opt => opt.MapFrom(c => c.Id))
            .ForMember(c => c.ClassName, opt => opt.MapFrom(c => c.Name))
            .ForMember(c => c.ProgramName, opt => opt.MapFrom(c => c.Program.Name))
            .ForMember(c => c.TotalModules, opt => opt.MapFrom(c => c.Program.Modules.Count))
            .ForMember(c => c.IsNew, opt => opt.MapFrom(w => w.IsNew))
            .ForMember(c => c.SessionName, opt => opt.MapFrom(w => w.Session.Name));

        CreateMap<Class, SubModuleDtoWithWorker>()
            .ForMember(c => c.Workers, opt => opt.MapFrom(w => w.ClassModulesWorkers));
        CreateMap<Class, GetUpdateIsNewInClassResponseDto>();

        CreateMap<Module, GetClassModuleResponseDto>();
        CreateMap<Module, SubModuleDtoWithWorker>()
            .ForMember(c => c.Workers, opt => opt.MapFrom(w => w.ClassModulesWorkers));

        CreateMap<Class, GetAllClassDto>();
    }
}
