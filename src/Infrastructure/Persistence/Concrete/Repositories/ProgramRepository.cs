namespace Space.Infrastructure.Persistence.Concrete;

internal class ProgramRepository : Repository<Program>, IProgramRepository
{
	public ProgramRepository(SpaceDbContext dbContext) : base(dbContext) { }
}
