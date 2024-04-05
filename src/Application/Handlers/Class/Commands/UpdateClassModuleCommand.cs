
namespace Space.Application.Handlers.Commands;

public class UpdateClassModuleCommand : IRequest
{
    public int Id { get; set; }
    public IEnumerable<UpdateClassModuleDto> Modules { get; set; } = null!;
    public IEnumerable<CreateClassExtraModuleRequestDto>? ExtraModules { get; set; }
    public IEnumerable<CreateClassNewExtraModuleRequestDto>? NewExtraModules { get; set; } = null!;
}
internal class UpdateClassModuleHandler : IRequestHandler<UpdateClassModuleCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    public UpdateClassModuleHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateClassModuleCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Include(c => c.Session)
            .ThenInclude(c => c.Details)
            .FirstOrDefaultAsync(c => c.Id == request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);

        //Existing modules and extra modules
        IEnumerable<Module> modulesDb = await _spaceDbContext.Modules
                   .ToListAsync(cancellationToken: cancellationToken);

        foreach (UpdateClassModuleDto item in request.Modules)
        {
            Module? module = modulesDb.FirstOrDefault(c => c.Id == item.ModuleId) ??
                throw new NotFoundException(nameof(Module), item.ModuleId);

            if (module.TopModuleId == null)
            {
                request.Modules.ToList().Remove(request.Modules.First(rm => rm.ModuleId == module.Id));
            }
        }
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


        foreach (UpdateClassModuleDto item in request.Modules)
        {
            await _spaceDbContext.ClassModulesWorkers.AddRangeAsync(item.Workers.Select(c => new ClassModulesWorker()
            {
                WorkerId = c.WorkerId,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                ModuleId = item.ModuleId,
                RoleId = c.RoleId,
                ClassId = @class.Id
            }), cancellationToken);
        }
        //--Extra Modules

        IEnumerable<ClassExtraModulesWorkers> classExtraModulesWorkers = await _spaceDbContext.ClassExtraModulesWorkers
            .Where(c => c.ClassId == request.Id)
            .ToListAsync(cancellationToken: cancellationToken);
        _spaceDbContext.ClassExtraModulesWorkers.RemoveRange(classExtraModulesWorkers);

        if (request.ExtraModules != null)
        {
            foreach (CreateClassExtraModuleRequestDto item in request.ExtraModules)
            {
                await _spaceDbContext.ClassExtraModulesWorkers.AddRangeAsync(item.Workers.Select(c => new ClassExtraModulesWorkers()
                {
                    WorkerId = c.WorkerId,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    ExtraModuleId = item.ExtraModuleId,
                    RoleId = c.RoleId,
                    ClassId = @class.Id
                }), cancellationToken);
            }
        }


        if (request.NewExtraModules != null && request.NewExtraModules.Any())
        {
            IEnumerable<ExtraModule> newExtraModules = request.NewExtraModules.Select(c => new ExtraModule()
            {
                Name = c.ExtraModuleName,
                ProgramId = @class.ProgramId,
                Hours = c.Hours,
                //Todo: Must be the biggest modules version
                Version = "1.0",
                ClassExtraModulesWorkers = c.Workers.Select(w => new ClassExtraModulesWorkers()
                {
                    WorkerId = w.WorkerId,
                    ClassId = @class.Id,
                    RoleId = w.RoleId,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                }).ToList()
            });

            await _spaceDbContext.ExtraModules.AddRangeAsync(newExtraModules, cancellationToken);

            DateOnly startDate = request.NewExtraModules.OrderBy(c => c.StartDate).First().StartDate;
            DateOnly endDate = request.NewExtraModules.OrderByDescending(c => c.EndDate).First().EndDate;


            if (@class.EndDate >= startDate)
            {
                startDate = @class.EndDate.Value.AddDays(1);
            }

            List<CreateClassSessionDto> sessions = @class.Session.Details.Select(c => new CreateClassSessionDto()
            {
                Category = c.Category,
                DayOfWeek = c.DayOfWeek,
                End = c.EndTime,
                Start = c.StartTime,
            }).ToList();

            List<DateOnly> holidays = await _unitOfWork.HolidayService.GetDatesAsync();
            int roomId = @class.RoomId ?? throw new NotFoundException(nameof(Room));
            List<ClassSession> newClassSessions = _unitOfWork.ClassSessionService.GenerateSessions(startDate, sessions, endDate, holidays, @class.Id, roomId);
            _spaceDbContext.ClassSessions.AddRange(newClassSessions);
            @class.EndDate = newClassSessions.OrderByDescending(c => c.Date).First().Date;

        }


        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}