namespace Space.Application.Handlers;

public class GetUsersQuery : IRequest<IEnumerable<GetUserPermissionGroupResponseDto>> { }

internal class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<GetUserPermissionGroupResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetUsersQueryHandler(ISpaceDbContext spaceDbContext, IMapper mapper)
    {
        _spaceDbContext = spaceDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetUserPermissionGroupResponseDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        List<Worker> users = await _spaceDbContext.Workers
        .Include(c => c.PermissionGroups)
        .ToListAsync();

        return _mapper.Map<IEnumerable<GetUserPermissionGroupResponseDto>>(users);
    }
}