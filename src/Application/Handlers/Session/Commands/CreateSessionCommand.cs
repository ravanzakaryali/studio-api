namespace Space.Application.Handlers;

public record CreateSessionCommand(string Name):IRequest<GetSessionResponseDto>;

internal class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, GetSessionResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public CreateSessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetSessionResponseDto> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        Session newSession = _mapper.Map<Session>(request);
        await _unitOfWork.SessionRepository.AddAsync(newSession);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetSessionResponseDto>(newSession);
    }
}
