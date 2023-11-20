namespace Space.Application.Handlers;

public record CreateSessionDetailCommand(Guid Id, CreateSessionDetailRequestDto Details) : IRequest<GetSessionWithDetailsResponseDto>;
internal class CreateSessionDetailCommandHandler : IRequestHandler<CreateSessionDetailCommand, GetSessionWithDetailsResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateSessionDetailCommandHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetSessionWithDetailsResponseDto> Handle(CreateSessionDetailCommand request, CancellationToken cancellationToken)
    {
        Session? session = await _spaceDbContext.Sessions
            .Include(c => c.Details)
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Session), request.Id);
        SessionDetail newSessionDetail = _mapper.Map<SessionDetail>(request.Details);
        newSessionDetail.SessionId = session.Id;
        await _spaceDbContext.SessionDetails.AddAsync(newSessionDetail);
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<GetSessionWithDetailsResponseDto>(session);
    }
}
