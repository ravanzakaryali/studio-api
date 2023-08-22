namespace Space.Application.Handlers;

public record CreateSessionDetailCommand(Guid Id, CreateSessionDetailRequestDto Details) : IRequest<GetSessionWithDetailsResponseDto>;
internal class CreateSessionDetailCommandHandler : IRequestHandler<CreateSessionDetailCommand, GetSessionWithDetailsResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public CreateSessionDetailCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetSessionWithDetailsResponseDto> Handle(CreateSessionDetailCommand request, CancellationToken cancellationToken)
    {
        Session? session = await _unitOfWork.SessionRepository.GetAsync(a=>a.Id == request.Id,includeProperties: "Details") ?? 
                throw new NotFoundException(nameof(Session),request.Id);
        SessionDetail newSessionDetail = _mapper.Map<SessionDetail>(request.Details);
        newSessionDetail.SessionId = session.Id;
        await _unitOfWork.SessionDetailRepository.AddAsync(newSessionDetail);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetSessionWithDetailsResponseDto>(session);
    }
}
