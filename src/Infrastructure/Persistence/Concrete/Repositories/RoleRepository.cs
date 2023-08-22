using Space.Application.Abstractions.Repositories;

namespace Space.Infrastructure.Persistence;

internal class RoleRepository : Repository<Role>, IRoleRepository
{
    private SpaceDbContext _dbContext;
    public RoleRepository(SpaceDbContext context) : base(context)
    {
        _dbContext = context;
    }
}

