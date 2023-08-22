namespace Space.Application.Mappers;

public class AuthMappers : Profile
{
    public AuthMappers()
    {
        CreateMap<RegisterCommand, RegisterDto>();
        CreateMap<RegisterDto, User>();
    }
}
