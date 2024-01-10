namespace Space.Application.Mappers;

public class ProgramMappers : Profile
{
	public ProgramMappers()
	{
        CreateMap<CreateProgramCommand, Program>();
        CreateMap<Program, GetAllProgramResponseDto>();
        CreateMap<Program, GetProgramResponseDto>();
    }
}
