namespace Space.Infrastructure.Persistence.Concrete;

internal class ModuleRepository : Repository<Module>, IModuleRepository
{
	private SpaceDbContext _dbContext;
	public ModuleRepository(SpaceDbContext dbContext) : base(dbContext) 
	{
		_dbContext = dbContext;
	}
	public async Task<bool> IsUnique(List<Module> modules)
	{
        List<string> moduleNames = modules.Select(a => a.Name).ToList();
        moduleNames.AddRange(modules.SelectMany(a => a.SubModules.Select(a => a.Name)).ToList());
		var a = await _dbContext.Modules.Where(a=>moduleNames.Contains(a.Name)).FirstOrDefaultAsync();
		return a != null;
	}
}
