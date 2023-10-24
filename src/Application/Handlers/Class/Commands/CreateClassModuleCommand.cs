using Microsoft.Extensions.FileProviders;
using Space.Application.Abstractions.Repositories;

namespace Space.Application.Handlers;

public class CreateClassModuleCommand : IRequest
{
    public Guid ClassId { get; set; }
    public IEnumerable<CreateClassModuleRequestDto> CreateClassModule { get; set; } = null!;
}

internal class CreateClassModuleCommandHandler : IRequestHandler<CreateClassModuleCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IClassRepository _classRepository;
    readonly IModuleRepository _moduleRepository;
    readonly IWorkerRepository _workerRepository;
    readonly IRoleRepository _roleRepository;
    readonly IClassModulesWorkerRepository _classModulesWorkerRepository;

    public CreateClassModuleCommandHandler(
        IUnitOfWork unitOfWork,
        IClassRepository classRepository,
        IModuleRepository moduleRepository,
        IWorkerRepository workerRepository,
        IRoleRepository roleRepository,
        IClassModulesWorkerRepository modulesWorkerRepository)
    {
        _unitOfWork = unitOfWork;
        _classRepository = classRepository;
        _moduleRepository = moduleRepository;
        _workerRepository = workerRepository;
        _roleRepository = roleRepository;
        _classModulesWorkerRepository = modulesWorkerRepository;
    }

    public async Task Handle(CreateClassModuleCommand request, CancellationToken cancellationToken)
    {

        Class? @class = await _classRepository.GetAsync(request.ClassId, tracking: false, "Program.Modules.SubModules")
            ?? throw new NotFoundException(nameof(Class), request.ClassId);


        IEnumerable<Guid> moduleIds = request.CreateClassModule.Select(c => c.ModuleId);
        IEnumerable<Module> modules = await _moduleRepository.GetAllAsync(c => moduleIds.Contains(c.Id), tracking: false);
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
        IEnumerable<Worker> workers = await _workerRepository.GetAllAsync(c => workerIds.Contains(c.Id));
        IEnumerable<Guid> existingWorkerIds = workers.Select(w => w.Id);
        IEnumerable<Guid> nonExistingWorkerIds = workerIds.Except(existingWorkerIds);
        if (nonExistingWorkerIds.Any())
            throw new NotFoundException(nameof(Worker), $"{string.Join(",", nonExistingWorkerIds)}");

        IEnumerable<Guid> roleIds = request.CreateClassModule.Select(c => c.RoleId);
        IEnumerable<Role> roles = await _roleRepository.GetAllAsync(c => roleIds.Contains(c.Id));
        IEnumerable<Guid> nonExistingRoleIds = roles.Select(w => w.Id);
        if (nonExistingRoleIds.Count() == 0)
            throw new NotFoundException(nameof(Role), $"{string.Join(",", nonExistingRoleIds)}");

        IEnumerable<ClassModulesWorker> classModulesWorker = await _classModulesWorkerRepository.GetAllAsync(c => c.ClassId == request.ClassId);
        _classModulesWorkerRepository.RemoveRange(classModulesWorker);

        await _classModulesWorkerRepository.AddRangeAsync(request.CreateClassModule.Select(c => new ClassModulesWorker()
        {
            WorkerId = c.WorkerId,
            ModuleId = c.ModuleId,
            RoleId = c.RoleId,
            ClassId = @class.Id
        }));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
