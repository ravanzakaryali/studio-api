namespace Space.Application.Handlers;

public record CreateSessionCommand(string Name) : IRequest<GetSessionResponseDto>;

internal class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, GetSessionResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISessionRepository _sessionRepository;

    public CreateSessionCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISessionRepository sessionRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionRepository = sessionRepository;
    }

    public async Task<GetSessionResponseDto> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        Session newSession = _mapper.Map<Session>(request);
        await _sessionRepository.AddAsync(newSession);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetSessionResponseDto>(newSession);
    }
}
