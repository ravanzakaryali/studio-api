namespace Space.Application.Handlers;

public class GetPermissionGroupQuery : IRequest<GetPermissionGroupDto>
{
    public int Id { get; set; }
}

internal class GetPermissionGroupHandler : IRequestHandler<GetPermissionGroupQuery, GetPermissionGroupDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetPermissionGroupHandler(ISpaceDbContext spaceDbContext, IMapper mapper)
    {
        _spaceDbContext = spaceDbContext;
        _mapper = mapper;
    }

    public async Task<GetPermissionGroupDto> Handle(GetPermissionGroupQuery request, CancellationToken cancellationToken)
    {
        PermissionGroup? permissionGroup = await _spaceDbContext.PermissionGroups
            .Include(pg => pg.Workers)
            .FirstOrDefaultAsync(pg => pg.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(PermissionGroup), request.Id);

        return _mapper.Map<GetPermissionGroupDto>(permissionGroup);
    }
}