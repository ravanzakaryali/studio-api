namespace Space.Application.Handlers;

public record GetAllRoomQuery : IRequest<IEnumerable<GetRoomResponseDto>>;

internal class GetAllRoomQueryHandler : IRequestHandler<GetAllRoomQuery, IEnumerable<GetRoomResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetAllRoomQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetRoomResponseDto>> Handle(GetAllRoomQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<GetRoomResponseDto>>(await _spaceDbContext.Rooms.ToListAsync());
    }
}
