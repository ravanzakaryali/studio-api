namespace Space.Application.Handlers;

public record DeleteSessionCommand(Guid Id) : IRequest<GetSessionResponseDto>;
internal class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand, GetSessionResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public DeleteSessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetSessionResponseDto> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        Session? session = await _unitOfWork.SessionRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Session), request.Id);
        _unitOfWork.SessionRepository.Remove(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetSessionResponseDto>(session);
    }
}
