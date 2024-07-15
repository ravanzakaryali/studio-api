namespace Space.Application.Handlers;

public class GetPermissionGroupWithUsersQuery : IRequest<GetPermissionGroupDto>
{
    public int Id { get; set; }
}

internal class GetPermissionGroupWithUsersHandler : IRequestHandler<GetPermissionGroupWithUsersQuery, GetPermissionGroupDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetPermissionGroupWithUsersHandler(ISpaceDbContext spaceDbContext, IMapper mapper)
    {
        _spaceDbContext = spaceDbContext;
        _mapper = mapper;
    }

    public async Task<GetPermissionGroupDto> Handle(GetPermissionGroupWithUsersQuery request, CancellationToken cancellationToken)
    {
        PermissionGroup? permissionGroup = await _spaceDbContext.PermissionGroups
            .Include(pg => pg.Workers)
            .FirstOrDefaultAsync(pg => pg.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(PermissionGroup), request.Id);

        return _mapper.Map<GetPermissionGroupDto>(permissionGroup);
    }
}