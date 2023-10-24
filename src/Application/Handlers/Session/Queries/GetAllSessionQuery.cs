namespace Space.Application.Handlers;

public class GetAllSessionQuery : IRequest<IEnumerable<GetSessionWithDetailsResponseDto>>
{
}

internal class GetAllSessionQueryHandler : IRequestHandler<GetAllSessionQuery, IEnumerable<GetSessionWithDetailsResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISessionRepository _sessionRepository;

    public GetAllSessionQueryHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ISessionRepository sessionRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionRepository = sessionRepository;
    }

    public async Task<IEnumerable<GetSessionWithDetailsResponseDto>> Handle(GetAllSessionQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Session> sessions = await _sessionRepository.GetAllAsync(includes: "Details");
        return _mapper.Map<IEnumerable<GetSessionWithDetailsResponseDto>>(sessions);
    }
}
