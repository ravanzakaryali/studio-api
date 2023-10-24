namespace Space.Application.Handlers;

public record DeleteSessionCommand(Guid Id) : IRequest<GetSessionResponseDto>;
internal class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand, GetSessionResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISessionRepository _sessionRepository;

    public DeleteSessionCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISessionRepository sessionRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _sessionRepository = sessionRepository;
    }

    public async Task<GetSessionResponseDto> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        Session? session = await _sessionRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Session), request.Id);
        _sessionRepository.Remove(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetSessionResponseDto>(session);
    }
}
