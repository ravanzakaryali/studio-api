using Microsoft.EntityFrameworkCore.Query;

namespace Space.Application.Handlers;

public record GetAllWorkerQuery(RoleEnum? Role) : IRequest<IEnumerable<GetWorkerDto>>;


internal class GetAllWorkerQueryHandler : IRequestHandler<GetAllWorkerQuery, IEnumerable<GetWorkerDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllWorkerQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetWorkerDto>> Handle(GetAllWorkerQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Worker> workers = await _spaceDbContext.Workers
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken: cancellationToken);

        List<PermissionGroup> permissionGroups = await _spaceDbContext.PermissionGroups.Include(c => c.Workers).ToListAsync(cancellationToken: cancellationToken);

        List<GetWorkerDto> response = workers.Select(w => new GetWorkerDto()
        {
            Id = w.Id,
            Name = w.Name!,
            Surname = w.Surname,
            Email = w.Email,
            LastPasswordUpdateDate = w.LastPasswordUpdateDate,
            Roles = permissionGroups.Where(pg => pg.Workers.Any(w => w.Id == w.Id)).Select(pg => pg.Name).ToList().Select(c => new GetRoleDto()
            {
                Name = c,
                Id = permissionGroups.First(pg => pg.Name == c).Id
            }),
        }).ToList();


        return response;
    }
}
