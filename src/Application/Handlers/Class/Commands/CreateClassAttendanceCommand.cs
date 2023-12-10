namespace Space.Application.Handlers;

public class CreateClassAttendanceCommand : IRequest
{
    public Guid ClassId { get; set; }
    public DateOnly Date { get; set; }
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; } = null!;
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
        //classın datalarını gətir
        Class @class = await _spaceDbContext.Classes
            .Include(c => c.Studies)
            .Include(c => c.Session)
            .ThenInclude(c => c.Details)
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

        //həmin günün class sessiona bax
        List<ClassGenerateSession> classSessions = await _spaceDbContext.ClassGenerateSessions
            .Where(c => c.Date == request.Date && c.ClassId == request.ClassId)
            .ToListAsync(cancellationToken: cancellationToken);

        List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();
        DateOnly classLastDate = await _unitOfWork.ClassSessionService.GetLastDateAsync(@class.Id);

        //əgər yoxdursa o zaman error qaytar
        List<Guid> requestModuleIds = request.HeldModules.Select(c => c.ModuleId).ToList();
        if (classSessions.Count != requestModuleIds.Count) throw new NotFoundException("Module not found");

        List<Module> module = await _spaceDbContext.Modules
            .Where(m => requestModuleIds.Contains(m.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        if (requestModuleIds.Count != module.Count) throw new NotFoundException("Modules not found");

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
            ClassGenerateSession? classSession = classSessions.Where(cs => cs.Category == session.Category).FirstOrDefault();
            if (classSession is null) continue;

            if (classSession.Status != ClassSessionStatus.Cancelled)
            {
                addTimeSheets.Add(new ClassTimeSheet()
                {
                    Attendances = session.Attendances.Select(c => new Attendance()
                    {
                        StudyId = c.StudentId,
                        Note = c.Note,
                        Status = classSession.TotalHours == c.TotalAttendanceHours
                                           ? StudentStatus.Attended
                                           : c.TotalAttendanceHours == 0
                                           ? StudentStatus.Absent
                                           : StudentStatus.Partial,
                        TotalAttendanceHours = c.TotalAttendanceHours
                    }).ToList(),
                    AttendancesWorkers = session.AttendancesWorkers.Select(wa => new AttendanceWorker()
                    {
                        WorkerId = wa.WorkerId,
                        TotalHours = wa.TotalHours,
                        TotalMinutes = wa.TotalMinutes,
                        AttendanceStatus = wa.AttendanceStatus,
                        RoleId = wa.RoleId,
                    }).ToList(),
                    TotalHours = classSession.TotalHours,
                    Category = classSession.Category,
                    ClassId = classSession.ClassId,
                    EndTime = classSession.EndTime,
                    Date = classSession.Date,
                    HeldModules = request.HeldModules.Select(hm => new HeldModule()
                    {
                        ModuleId = hm.ModuleId,
                        TotalHours = hm.TotalHours,
                    }).ToList(),
                    StartTime = classSession.StartTime,
                    Status = classSession.Status,
                });
            }
            else
            {
                session.Status = ClassSessionStatus.Cancelled;

                DateOnly date2 = classLastDate.AddDays(1);
                DayOfWeek startDateDayOfWeek = date2.DayOfWeek;
                while (!@class.Session.Details.Select(c => c.DayOfWeek).Any(c => date2.DayOfWeek == c))
                {
                    date2 = date2.AddDays(1);
                    startDateDayOfWeek = date2.DayOfWeek;
                }
                List<ClassGenerateSession> generateClassSessions = _unitOfWork.ClassSessionService.GenerateSessions(
                    classSession.TotalHours, request.Sessions.Select(r => new CreateClassSessionDto()
                    {
                        Category = r.Category,
                        DayOfWeek = startDateDayOfWeek,
                        End = classSession.EndTime,
                        Start = classSession.StartTime
                    }).ToList(),
                    date2,
                    holidayDates,
                    @class.Id,
                    classSession.RoomId!.Value);

                await _spaceDbContext.ClassGenerateSessions.AddRangeAsync(generateClassSessions, cancellationToken);
            }
        }

        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
