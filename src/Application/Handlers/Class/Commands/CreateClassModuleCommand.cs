using DocumentFormat.OpenXml.Drawing;

namespace Space.Application.Handlers;

public class CreateClassModuleCommand : IRequest
{
    public int ClassId { get; set; }
    public IEnumerable<CreateClassModuleRequestDto> CreateClassModule { get; set; } = null!;
}


internal class CreateClassModuleCommandHandler : IRequestHandler<CreateClassModuleCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMediator _mediator;

    public CreateClassModuleCommandHandler(
        IUnitOfWork unitOfWork,
        ISpaceDbContext spaceDbContext,
        IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
        _mediator = mediator;
    }

    public async Task Handle(CreateClassModuleCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .ThenInclude(c => c.SubModules)
            .Where(c => c.Id == request.ClassId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Class), request.ClassId);

        IEnumerable<int> moduleIds = request.CreateClassModule.Select(c => c.ModuleId);
        IEnumerable<Module> modules = await _spaceDbContext.Modules
            .Where(c => moduleIds.Contains(c.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        IEnumerable<int> existingModuleIds = modules.Select(m => m.Id);
        IEnumerable<int> nonExistingModuleIds = moduleIds.Except(existingModuleIds);
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

        // IEnumerable<int> workerIds = request.CreateClassModule.Where(c => c.WorkerId != null || c.WorkerId == 0).Select(c => c.WorkerId!.Value);
        // if (@class.Program.Modules.Any(c => moduleIds.Contains(c.Id))) throw new NotFoundException("Modules not found in modules of class program");
        // IEnumerable<Worker> workers = await _spaceDbContext.Workers
        //     .Where(c => workerIds.Contains(c.Id))
        //     .ToListAsync();

        // IEnumerable<int> existingWorkerIds = workers.Select(w => w.Id);
        // IEnumerable<int> nonExistingWorkerIds = workerIds.Except(existingWorkerIds);
        // if (nonExistingWorkerIds.Any())
        //     throw new NotFoundException(nameof(Worker), $"{string.Join(",", nonExistingWorkerIds)}");

        // IEnumerable<int> roleIds = request.CreateClassModule.Where(c => c.RoleId != null || c.RoleId == 0).Select(c => c.RoleId!.Value);
        // IEnumerable<Role> roles = await _spaceDbContext.Roles.Where(c => roleIds.Contains(c.Id)).ToListAsync(cancellationToken: cancellationToken);
        // IEnumerable<int> nonExistingRoleIds = roles.Select(w => w.Id);
        // if (!nonExistingRoleIds.Any())
        //     throw new NotFoundException(nameof(Role), $"{string.Join(",", nonExistingRoleIds)}");

        IEnumerable<ClassModulesWorker> classModulesWorker = await _spaceDbContext.ClassModulesWorkers
            .Where(c => c.ClassId == request.ClassId)
            .ToListAsync(cancellationToken: cancellationToken);

        _spaceDbContext.ClassModulesWorkers.RemoveRange(classModulesWorker);

        await _spaceDbContext.ClassModulesWorkers.AddRangeAsync(request.CreateClassModule
        .Where(c => c.WorkerId != 0)
        .Select(c =>
        {
            return new ClassModulesWorker()
            {
                WorkerId = c.WorkerId!.Value,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ModuleId = c.ModuleId,
                RoleId = c.RoleId,
                ClassId = @class.Id
            };
        }), cancellationToken);

        await _mediator.Send(new CreateClassSessionCommand()
        {
            ClassId = request.ClassId,
            SessionId = @class.SessionId
        }, cancellationToken);


        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
