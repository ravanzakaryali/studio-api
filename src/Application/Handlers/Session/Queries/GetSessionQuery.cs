namespace Space.Application.Handlers;

public record GetSessionQuery(int Id) : IRequest<GetSessionWithDetailsResponseDto>;

internal class GetSessionQueryHandler : IRequestHandler<GetSessionQuery, GetSessionWithDetailsResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetSessionQueryHandler(
        IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetSessionWithDetailsResponseDto> Handle(GetSessionQuery request, CancellationToken cancellationToken)
    {
        Session? session = await _spaceDbContext.Sessions
            .Include(c => c.Details)
            .Where(c => c.Id == request.Id).FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Session), request.Id);
        return _mapper.Map<GetSessionWithDetailsResponseDto>(session);
    }
}
