namespace Space.Application.Handlers;

public record CreateSessionCommand(string Name) : IRequest<GetSessionResponseDto>;

internal class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, GetSessionResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateSessionCommandHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetSessionResponseDto> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
    {
        Session newSession = _mapper.Map<Session>(request);
        await _spaceDbContext.Sessions.AddAsync(newSession);
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<GetSessionResponseDto>(newSession);
    }
}
