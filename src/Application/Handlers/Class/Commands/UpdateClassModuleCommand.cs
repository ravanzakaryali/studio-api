
namespace Space.Application.Handlers.Commands;

public class UpdateClassModuleCommand : IRequest
{
    public int Id { get; set; }
    public IEnumerable<CreateClassModuleRequestDto> Modules { get; set; } = null!;
    public IEnumerable<CreateClassExtraModuleRequestDto>? ExtraModules { get; set; }
    public IEnumerable<CreateClassNewExtraModuleRequestDto>? NewExtraModules { get; set; } = null!;
}
internal class UpdateClassModuleHandler : IRequestHandler<UpdateClassModuleCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;
    public UpdateClassModuleHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(UpdateClassModuleCommand request, CancellationToken cancellationToken)
    {
        Class @class = _spaceDbContext.Classes.Find(request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);

        //Existing modules and extra modules

        IEnumerable<int> moduleIds = request.Modules.Select(c => c.ModuleId);
        IEnumerable<Module> modules = await _spaceDbContext.Modules
            .Where(c => moduleIds.Contains(c.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (Module module in modules)
        {
            if (module.TopModuleId == null)
            {
                request.Modules.ToList().Remove(request.Modules.First(rm => rm.ModuleId == module.Id));
            }
        }

        //--Modules
        IEnumerable<ClassModulesWorker> classModulesWorker = await _spaceDbContext.ClassModulesWorkers
            .Where(c => c.ClassId == request.Id)
            .ToListAsync(cancellationToken: cancellationToken);
        _spaceDbContext.ClassModulesWorkers.RemoveRange(classModulesWorker);

        await _spaceDbContext.ClassModulesWorkers.AddRangeAsync(request.Modules.Select(c => new ClassModulesWorker()
        {
            WorkerId = c.WorkerId,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            ModuleId = c.ModuleId,
            RoleId = c.RoleId,
            ClassId = @class.Id
        }), cancellationToken);

        //--Extra Modules
        IEnumerable<ClassExtraModulesWorkers> classExtraModulesWorkers = await _spaceDbContext.ClassExtraModulesWorkers
            .Where(c => c.ClassId == request.Id)
            .ToListAsync(cancellationToken: cancellationToken);
        _spaceDbContext.ClassExtraModulesWorkers.RemoveRange(classExtraModulesWorkers);
        if (request.ExtraModules != null)
        {
            await _spaceDbContext.ClassExtraModulesWorkers.AddRangeAsync(request.ExtraModules.Select(c => new ClassExtraModulesWorkers()
            {
                WorkerId = c.WorkerId,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ExtraModuleId = c.ExtraModuleId,
                RoleId = c.RoleId,
                ClassId = @class.Id
            }), cancellationToken);
        }


        if (request.NewExtraModules != null)
        {
            IEnumerable<ExtraModule> newExtraModules = request.NewExtraModules.Select(c => new ExtraModule()
            {
                Name = c.ExtraModuleName,
                ProgramId = @class.ProgramId,
                Hours = c.Hours,
                //Todo: Must be the biggest modules version
                Version = "1.0",
                ClassExtraModulesWorkers = new List<ClassExtraModulesWorkers>()
            {
                new()
                {
                    WorkerId = c.WorkerId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    RoleId = c.RoleId,
                    ClassId = @class.Id
                }
            }
            });

            await _spaceDbContext.ExtraModules.AddRangeAsync(newExtraModules, cancellationToken);
        }


        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}