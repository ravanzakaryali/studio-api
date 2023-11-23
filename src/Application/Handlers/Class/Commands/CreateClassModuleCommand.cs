namespace Space.Application.Handlers;

public class CreateClassModuleCommand : IRequest
{
    public Guid ClassId { get; set; }
    public IEnumerable<CreateClassModuleRequestDto> CreateClassModule { get; set; } = null!;
}

internal class CreateClassModuleCommandHandler : IRequestHandler<CreateClassModuleCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateClassModuleCommandHandler(
        IUnitOfWork unitOfWork,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateClassModuleCommand request, CancellationToken cancellationToken)
    {

        Class? @class = await _spaceDbContext.Classes
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .ThenInclude(c => c.SubModules)
            .Where(c => c.Id == request.ClassId)
            .FirstOrDefaultAsync()
                ?? throw new NotFoundException(nameof(Class), request.ClassId);


        IEnumerable<Guid> moduleIds = request.CreateClassModule.Select(c => c.ModuleId);
        IEnumerable<Module> modules = await _spaceDbContext.Modules
            .Where(c => moduleIds.Contains(c.Id))
            .ToListAsync();
        IEnumerable<Guid> existingModuleIds = modules.Select(m => m.Id);
        IEnumerable<Guid> nonExistingModuleIds = moduleIds.Except(existingModuleIds);
        if (nonExistingModuleIds.Any())
            throw new NotFoundException(nameof(Module), $"{string.Join(",", nonExistingModuleIds)}");

        //if(modules.Any(m=>m.TopModuleId == null))
        //{
        //    throw new Exception("When adding a module to a class, the parent module ID should not.");
        //}

        foreach (Module module in modules)
        {
            if (module.TopModuleId == null)
            {
                request.CreateClassModule.ToList().Remove(request.CreateClassModule.First(rm => rm.ModuleId == module.Id));
            }
        }


        IEnumerable<Guid> workerIds = request.CreateClassModule.Select(c => c.WorkerId);
        //if (@class.Program.Modules.Any(c => moduleIds.Contains(c.Id))) throw new NotFoundException("Modules not found in modules of class program");
        IEnumerable<Worker> workers = await _spaceDbContext.Workers
            .Where(c => workerIds.Contains(c.Id))
            .ToListAsync();
        IEnumerable<Guid> existingWorkerIds = workers.Select(w => w.Id);
        IEnumerable<Guid> nonExistingWorkerIds = workerIds.Except(existingWorkerIds);
        if (nonExistingWorkerIds.Any())
            throw new NotFoundException(nameof(Worker), $"{string.Join(",", nonExistingWorkerIds)}");

        IEnumerable<Guid> roleIds = request.CreateClassModule.Select(c => c.RoleId);
        IEnumerable<Role> roles = await _spaceDbContext.Roles.Where(c => roleIds.Contains(c.Id)).ToListAsync();
        IEnumerable<Guid> nonExistingRoleIds = roles.Select(w => w.Id);
        if (nonExistingRoleIds.Count() == 0)
            throw new NotFoundException(nameof(Role), $"{string.Join(",", nonExistingRoleIds)}");

        IEnumerable<ClassModulesWorker> classModulesWorker = await _spaceDbContext.ClassModulesWorkers
            .Where(c => c.ClassId == request.ClassId)
            .ToListAsync();
        _spaceDbContext.ClassModulesWorkers.RemoveRange(classModulesWorker);

        await _spaceDbContext.ClassModulesWorkers.AddRangeAsync(request.CreateClassModule.Select(c => new ClassModulesWorker()
        {
            WorkerId = c.WorkerId,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            ModuleId = c.ModuleId,
            RoleId = c.RoleId,
            ClassId = @class.Id
        }));
        await _spaceDbContext.SaveChangesAsync();
    }
}
