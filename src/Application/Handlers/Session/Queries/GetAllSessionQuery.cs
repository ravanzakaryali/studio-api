namespace Space.Application.Handlers;

public class GetAllSessionQuery : IRequest<IEnumerable<GetSessionWithDetailsResponseDto>>
{
}

internal class GetAllSessionQueryHandler : IRequestHandler<GetAllSessionQuery, IEnumerable<GetSessionWithDetailsResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetAllSessionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetSessionWithDetailsResponseDto>> Handle(GetAllSessionQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Session> sessions = await _unitOfWork.SessionRepository.GetAllAsync(includes: "Details");
        return _mapper.Map<IEnumerable<GetSessionWithDetailsResponseDto>>(sessions);
    }
}
