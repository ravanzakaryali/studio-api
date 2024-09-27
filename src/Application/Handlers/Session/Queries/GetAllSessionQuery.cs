namespace Space.Application.Handlers;

public class GetAllSessionQuery : IRequest<IEnumerable<GetSessionResponseDto>>
{
}

internal class GetAllSessionQueryHandler : IRequestHandler<GetAllSessionQuery, IEnumerable<GetSessionResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetAllSessionQueryHandler(
        IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetSessionResponseDto>> Handle(GetAllSessionQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Session> sessions = await _spaceDbContext.Sessions
            .Include(c => c.Details)
            .ToListAsync();
        return _mapper.Map<IEnumerable<GetSessionResponseDto>>(sessions);
    }
}
