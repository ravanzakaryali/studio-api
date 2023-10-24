namespace Space.Application.Handlers;

public record DeleteSessionDetailCommand(Guid SessionId, Guid SessionDetailId) : IRequest<GetSessionWithDetailsResponseDto>;

internal class DeleteSessionDetailCommandHandler : IRequestHandler<DeleteSessionDetailCommand, GetSessionWithDetailsResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISessionRepository _sessionRepository;

    public DeleteSessionDetailCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        ISessionRepository sessionRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionRepository = sessionRepository;
    }

    public async Task<GetSessionWithDetailsResponseDto> Handle(DeleteSessionDetailCommand request, CancellationToken cancellationToken)
    {
        Session? session = await _sessionRepository.GetAsync(request.SessionId, include: "Details")
            ?? throw new NotFoundException(nameof(Session), request.SessionId);

        SessionDetail? sessionDetail = session.Details.Where(sd => sd.Id == request.SessionDetailId).FirstOrDefault()
            ?? throw new NotFoundException(nameof(SessionDetail), request.SessionDetailId);
        session.Details.Remove(sessionDetail);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetSessionWithDetailsResponseDto>(session);
    }
}
