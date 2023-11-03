namespace Space.Application.Handlers;

public class GetAllSessionQuery : IRequest<IEnumerable<GetSessionWithDetailsResponseDto>>
{
}

internal class GetAllSessionQueryHandler : IRequestHandler<GetAllSessionQuery, IEnumerable<GetSessionWithDetailsResponseDto>>
{
    readonly IMapper _mapper;
    readonly ISessionRepository _sessionRepository;

    public GetAllSessionQueryHandler(
        IMapper mapper,
        ISessionRepository sessionRepository)
    {
        _mapper = mapper;
        _sessionRepository = sessionRepository;
    }

    public async Task<IEnumerable<GetSessionWithDetailsResponseDto>> Handle(GetAllSessionQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Session> sessions = await _sessionRepository.GetAllAsync(includes: "Details");
        return _mapper.Map<IEnumerable<GetSessionWithDetailsResponseDto>>(sessions);
    }
}
