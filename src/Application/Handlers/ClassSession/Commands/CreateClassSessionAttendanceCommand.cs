namespace Space.Application.Handlers;

public class CreateClassSessionAttendanceCommand : IRequest
{
    public Guid ClassId { get; set; }

    public Guid ModuleId { get; set; }
    public DateTime Date { get; set; }
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; } = null!;
}

internal class UpdateClassSessionAttendanceCommandHandler : IRequestHandler<CreateClassSessionAttendanceCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;
    readonly ICurrentUserService _currentUserService;

    public UpdateClassSessionAttendanceCommandHandler(
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task Handle(CreateClassSessionAttendanceCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId == null) throw new AutheticationException();

        Worker? worker = await _spaceDbContext.Workers.FindAsync(_currentUserService.UserId) ??
                throw new AutheticationException();

        Class? @class = await _spaceDbContext.Classes
            .Include(c => c.Studies)
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .ThenInclude(c => c.SubModules)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Worker)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Role)
            .Where(c => c.Id == request.ClassId)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.ClassId);


        Module module = await _spaceDbContext.Modules.FindAsync(request.ModuleId) ??
                throw new NotFoundException(nameof(Module), request.ModuleId);

        DateTime lastDate = await _unitOfWork.ClassSessionService
            .GetLastDateAsync(@class.Id);

        List<Module> modules = @class.Program.Modules
            .OrderBy(m => m.Version)
            .Where(m => m.TopModuleId != null || m.SubModules!.Any())
            .ToList();

        Module? currentModule = await _unitOfWork.ModuleService
            .GetCurrentModuleAsync(@class, request.Date);

        IEnumerable<WokerDto> currentModuleWorkers = @class.ClassModulesWorkers
            .Where(c => c.ModuleId == currentModule?.Id)
            .Distinct(new GetModulesWorkerComparer())
            .Select(c => new WokerDto()
            {
                Id = c.WorkerId,
                RoleName = c.Role?.Name
            });

        List<Guid> studentIds = request.Sessions
                .SelectMany(s => s.Attendances)
                .Select(a => a.StudentId)
                .ToList();

        List<Study> ClassStudiesExsist = @class.Studies
            .Where(c => !studentIds.Contains(c.Id)).ToList();

        IEnumerable<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.Date == request.Date && c.ClassId == request.ClassId)
            .Include(c => c.Attendances)
            .Include(c => c.AttendancesWorkers)
            .ToListAsync();

        await _unitOfWork.ClassSessionService.GenerateAttendanceAsync(request.Sessions, classSessions, request.ModuleId);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
