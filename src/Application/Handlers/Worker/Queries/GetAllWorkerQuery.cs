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
        IQueryable<Worker> query = _spaceDbContext.Workers
            .Include(c => c.UserRoles)
            .ThenInclude(c => c.Role)
            .OrderBy(c => c.Name)
            .AsQueryable();


        if (request.Role != null)
        {
            query = query.Where(c => c.UserRoles.Any(c => c.Role.Name == request.Role.ToString()));
        }

        var response = await query.Select(w => new GetWorkerDto()
        {
            Id = w.Id,
            Name = w.Name!,
            Surname = w.Surname,
            Email = w.Email,
            LastPasswordUpdateDate = w.LastPasswordUpdateDate,
            Roles = w.UserRoles.Select(c => new GetRoleDto()
            {
                Id = c.Role.Id,
                Name = c.Role.Name
            })
        }).ToListAsync(cancellationToken: cancellationToken);


        return response;
    }
}
