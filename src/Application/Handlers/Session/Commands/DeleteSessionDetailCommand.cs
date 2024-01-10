namespace Space.Application.Handlers;

public record DeleteSessionDetailCommand(int SessionId, int SessionDetailId) : IRequest<GetSessionWithDetailsResponseDto>;

internal class DeleteSessionDetailCommandHandler : IRequestHandler<DeleteSessionDetailCommand, GetSessionWithDetailsResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public DeleteSessionDetailCommandHandler(
        IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetSessionWithDetailsResponseDto> Handle(DeleteSessionDetailCommand request, CancellationToken cancellationToken)
    {
        Session? session = await _spaceDbContext.Sessions
            .Include(c => c.Details)
            .Where(c => c.Id == request.SessionId)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Session), request.SessionId);

        SessionDetail? sessionDetail = session.Details.Where(sd => sd.Id == request.SessionDetailId).FirstOrDefault()
            ?? throw new NotFoundException(nameof(SessionDetail), request.SessionDetailId);
        session.Details.Remove(sessionDetail);
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<GetSessionWithDetailsResponseDto>(session);
    }
}
