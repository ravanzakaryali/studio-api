using Space.Application.Abstractions;

namespace Space.Application.Handlers;

public class CreateClassModuleSessionCommand : IRequest
{
    public Guid ClassId { get; set; }
    public CreateClassModuleSessionRequestDto CreateClassModuleSessionDto { get; set; } = null!;
}
internal class CreateClassModuleSessionHandler : IRequestHandler<CreateClassModuleSessionCommand>
{
    private readonly IMediator _mediator;
    private readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;
    public CreateClassModuleSessionHandler(
        IMediator mediator,
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    //Todo: Request module change - High
    public async Task Handle(CreateClassModuleSessionCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
           .Include(c => c.Session)
           .ThenInclude(c => c.Details)
           .Include(c => c.Program)
           .ThenInclude(c => c.Modules)
           .ThenInclude(c => c.SubModules)
           .Where(c => c.Id == request.ClassId)
           .FirstOrDefaultAsync() ??
               throw new NotFoundException(nameof(Class), request.ClassId);
        //Create Modules


        IEnumerable<Guid> moduleIds = request.CreateClassModuleSessionDto.Modules.Select(c => c.ModuleId);
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
                request.CreateClassModuleSessionDto.Modules.ToList().Remove(request.CreateClassModuleSessionDto.Modules.First(rm => rm.ModuleId == module.Id));
            }
        }


        IEnumerable<Guid> workerIds = request.CreateClassModuleSessionDto.Modules.Select(c => c.WorkerId);
        //if (@class.Program.Modules.Any(c => moduleIds.Contains(c.Id))) throw new NotFoundException("Modules not found in modules of class program");
        IEnumerable<Worker> workers = await _spaceDbContext.Workers
            .Where(c => workerIds.Contains(c.Id))
            .ToListAsync();
        IEnumerable<Guid> existingWorkerIds = workers.Select(w => w.Id);
        IEnumerable<Guid> nonExistingWorkerIds = workerIds.Except(existingWorkerIds);
        if (nonExistingWorkerIds.Any())
            throw new NotFoundException(nameof(Worker), $"{string.Join(",", nonExistingWorkerIds)}");

        IEnumerable<Guid> roleIds = request.CreateClassModuleSessionDto.Modules.Select(c => c.RoleId);
        IEnumerable<Role> roles = await _spaceDbContext.Roles.Where(c => roleIds.Contains(c.Id)).ToListAsync();
        IEnumerable<Guid> nonExistingRoleIds = roles.Select(w => w.Id);
        if (nonExistingRoleIds.Count() == 0)
            throw new NotFoundException(nameof(Role), $"{string.Join(",", nonExistingRoleIds)}");

        IEnumerable<ClassModulesWorker> classModulesWorker = await _spaceDbContext.ClassModulesWorkers
            .Where(c => c.ClassId == request.ClassId)
            .ToListAsync();
        _spaceDbContext.ClassModulesWorkers.RemoveRange(classModulesWorker);

        await _spaceDbContext.ClassModulesWorkers.AddRangeAsync(request.CreateClassModuleSessionDto.Modules.Select(c => new ClassModulesWorker()
        {
            WorkerId = c.WorkerId,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            ModuleId = c.ModuleId,
            RoleId = c.RoleId,
            ClassId = @class.Id
        }));

        _spaceDbContext.ClassSessions.RemoveRange(await _spaceDbContext.ClassSessions.Where(cr => cr.ClassId == @class.Id).ToListAsync());

        Session session = await _spaceDbContext.Sessions
            .Include(c => c.Details)
            .Where(c => c.Id == request.CreateClassModuleSessionDto.SessionId)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Session), request.CreateClassModuleSessionDto.SessionId);

        if (@class.StartDate == null || @class.RoomId == null)
            throw new Exception("Class start date or room null");

        List<CreateClassSessionDto> sessions = session.Details
            .Select(c => new CreateClassSessionDto()
            {
                Category = c.Category,
                DayOfWeek = c.DayOfWeek,
                End = c.EndTime,
                Start = c.StartTime
            }).ToList();

        List<DayOfWeek> selectedDays = sessions.Select(c => c.DayOfWeek).ToList();

        List<DateTime> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();


        List<ClassTimeSheet> classSessions = _unitOfWork.ClassSessionService.GenerateSessions(
                                                                                       @class.Program.TotalHours,
                                                                                       sessions,
                                                                                       @class.StartDate.Value,
                                                                                       holidayDates,
                                                                                       @class.Id,
                                                                                       @class.RoomId.Value);

        @class.EndDate = classSessions.Max(c => c.Date).Date;
        await _spaceDbContext.ClassSessions.AddRangeAsync(classSessions);
        await _spaceDbContext.SaveChangesAsync();
    }
}
