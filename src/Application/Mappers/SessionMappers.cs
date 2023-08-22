namespace Space.Application.Mappers;

public class SessionMappers : Profile
{
    public SessionMappers()
    {
        CreateMap<CreateSessionCommand, Session>();
        CreateMap<Session, GetSessionResponseDto>();
        CreateMap<CreateSessionDetailRequestDto, SessionDetail>();
        CreateMap<Session, GetSessionWithDetailsResponseDto>();
        CreateMap<SessionDetail, GetDetailsResponseDto>();
        CreateMap<SessionDetail, GetSessionDetailDto>();
    }
}
