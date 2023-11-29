using Space.Domain.Entities;

namespace Space.Application.Handlers;

public class CreateClassAttendanceCommand : IRequest
{
    public Guid ClassId { get; set; }
    public DateOnly Date { get; set; }
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; } = null!;
    //Todo: Change name 
    public ICollection<CreateAttendanceModuleRequestDto> HeldModules { get; set; } = null!;
}
internal class CreateClassAttendanceCommandHandler : IRequestHandler<CreateClassAttendanceCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;

    public CreateClassAttendanceCommandHandler(
        IUnitOfWork unitOfWork,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateClassAttendanceCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Include(c => c.Studies)
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .ThenInclude(c => c.SubModules)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Worker)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Role)
            .Where(c => c.Id == request.ClassId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.ClassId);

        List<ClassSessions> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.Date == request.Date && c.ClassId == request.ClassId)
            .ToListAsync(cancellationToken: cancellationToken);

        List<Guid> moduleIds = request.HeldModules.Select(c => c.ModuleId).ToList();

        List<Module> module = await _spaceDbContext.Modules.Where(request.HeldModules.) ??

        List<Module> modules = @class.Program.Modules
            .OrderBy(m => m.Version)
            .Where(m => m.TopModuleId != null ||
                        !m.SubModules!.Any())
            .ToList();

        List<Guid> studentIds = request.Sessions
                .SelectMany(s => s.Attendances)
                .Select(a => a.StudentId)
                .ToList();

        if (@class.Studies.Any(c => !studentIds.Contains(c.Id))) throw new NotFoundException("Student not found");

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.Date == request.Date && c.ClassId == request.ClassId)
            .ToListAsync(cancellationToken: cancellationToken);

        if (classTimeSheets.Any())
            _spaceDbContext.ClassTimeSheets.RemoveRange(classTimeSheets);

        List<ClassTimeSheet> addTimeSheets = new();
        foreach (UpdateAttendanceCategorySessionDto session in request.Sessions)
        {
            ClassTimeSheet? matchingClassTimeSheet = classTimeSheets.Where(cs => cs.Category == session.Category).FirstOrDefault();
            if (matchingClassTimeSheet is null) continue;
            addTimeSheets.Add(new ClassTimeSheet()
            {
                Attendances = session.Attendances.Select(c => new Attendance()
                {
                    StudyId = c.StudentId,
                    Note = c.Note,
                    Status = matchingClassTimeSheet.TotalHours == c.TotalAttendanceHours
                                       ? StudentStatus.Attended
                                       : c.TotalAttendanceHours == 0
                                       ? StudentStatus.Absent
                                       : StudentStatus.Partial,
                    TotalAttendanceHours = c.TotalAttendanceHours
                }).ToList(),
                AttendancesWorkers = session.AttendancesWorkers.Select(wa => new AttendanceWorker()
                {
                    WorkerId = wa.WorkerId,
                    TotalAttendanceHours = wa.IsAttendance ? matchingClassTimeSheet.TotalHours : 0,
                    RoleId = wa.RoleId,
                }).ToList(),
                TotalHours = matchingClassTimeSheet.TotalHours,
                Category = matchingClassTimeSheet.Category,
                ClassId = matchingClassTimeSheet.ClassId,
                EndTime = matchingClassTimeSheet.EndTime,
                Date = matchingClassTimeSheet.Date,
                HeldModules = //db dən tap əlavə et,
                StartTime = matchingClassTimeSheet.StartTime,
                Status = matchingClassTimeSheet.Status,
            });
        }

        //varsa sil 
        //daha sonra hər halda yarat
        await _spaceDbContext.SaveChangesAsync();
    }
}
