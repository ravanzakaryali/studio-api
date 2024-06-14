namespace Space.Application.Handlers;

public class GetPermissionGroupQuery : IRequest<PermissionGroupDto>
{
    public int Id { get; set; }
}

internal class GetPermissionGroupHandler : IRequestHandler<GetPermissionGroupQuery, PermissionGroupDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetPermissionGroupHandler(ISpaceDbContext spaceDbContext, IMapper mapper)
    {
        _spaceDbContext = spaceDbContext;
        _mapper = mapper;
    }

    public async Task<PermissionGroupDto> Handle(GetPermissionGroupQuery request, CancellationToken cancellationToken)
    {
        PermissionGroup? permissionGroup = await _spaceDbContext.PermissionGroups
            .FirstOrDefaultAsync(pg => pg.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(PermissionGroup), request.Id);

        return _mapper.Map<PermissionGroupDto>(permissionGroup);
    }
}