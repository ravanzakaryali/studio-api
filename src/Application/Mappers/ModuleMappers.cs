using AutoMapper;

namespace Space.Application.Mappers;

public class ModuleMappers : Profile
{
    public ModuleMappers()
    {
        CreateMap<ModuleDto, Module>();
        CreateMap<SubModuleDto, Module>().ReverseMap();
        CreateMap<CreateSubModuleDto, Module>().ReverseMap();
        CreateMap<Module, GetModuleDto>().ReverseMap();
    }
}
