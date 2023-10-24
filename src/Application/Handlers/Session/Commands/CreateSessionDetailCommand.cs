namespace Space.Application.Handlers;

public record CreateSessionDetailCommand(Guid Id, CreateSessionDetailRequestDto Details) : IRequest<GetSessionWithDetailsResponseDto>;
internal class CreateSessionDetailCommandHandler : IRequestHandler<CreateSessionDetailCommand, GetSessionWithDetailsResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISessionRepository _sessionRepository;
    readonly ISessionDetailRepository _sessionDetailRepository;

    public CreateSessionDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ISessionRepository sessionRepository, ISessionDetailRepository sessionDetailRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionRepository = sessionRepository;
        _sessionDetailRepository = sessionDetailRepository;
    }

    public async Task<GetSessionWithDetailsResponseDto> Handle(CreateSessionDetailCommand request, CancellationToken cancellationToken)
    {
        Session? session = await _sessionRepository.GetAsync(a => a.Id == request.Id, includeProperties: "Details") ??
                throw new NotFoundException(nameof(Session), request.Id);
        SessionDetail newSessionDetail = _mapper.Map<SessionDetail>(request.Details);
        newSessionDetail.SessionId = session.Id;
        await _sessionDetailRepository.AddAsync(newSessionDetail);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetSessionWithDetailsResponseDto>(session);
    }
}
