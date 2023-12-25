namespace Space.Application.Handlers;

public class CreateClassSessionAttendanceCommand : IRequest
{
    public int ClassId { get; set; }
    public DateOnly Date { get; set; }
    public ICollection<CreateAttendanceModuleRequestDto>? HeldModules { get; set; } = null;
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; } = null!;
}

internal class UpdateClassSessionAttendanceCommandHandler
    : IRequestHandler<CreateClassSessionAttendanceCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;

    public UpdateClassSessionAttendanceCommandHandler(
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork
    )
    {
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        CreateClassSessionAttendanceCommand request,
        CancellationToken cancellationToken
    )
    {
        Class @class =
            await _spaceDbContext
                .Classes
                .Include(c => c.Studies)
                .Include(c => c.Program)
                .ThenInclude(c => c.Modules)
                .ThenInclude(c => c.SubModules)
                .Include(c => c.ClassModulesWorkers)
                .ThenInclude(c => c.Worker)
                .Include(c => c.ClassModulesWorkers)
                .ThenInclude(c => c.Role)
                .Where(c => c.Id == request.ClassId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        //həmin günün class sessiona bax
        List<ClassSession> classSessions = await _spaceDbContext
            .ClassSessions
            .Where(c => c.Date == request.Date && c.ClassId == request.ClassId)
            .ToListAsync(cancellationToken: cancellationToken);

        //əgər yoxdursa o zaman error qaytar
        if (request.HeldModules != null)
        {
            List<int> requestModuleIds = request.HeldModules.Select(c => c.ModeuleId).ToList();
            // if (classSessions.Count != requestModuleIds.Count)
            //     throw new NotFoundException("Module not found");
            List<Module> module = await _spaceDbContext
                .Modules
                .Where(m => requestModuleIds.Contains(m.Id))
                .ToListAsync(cancellationToken: cancellationToken);

            if (requestModuleIds.Count != module.Count)
                throw new NotFoundException("Modules not found");
        }

        //ders cancelled olursa bu işləməlidi
        List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();
        DateOnly classLastDate = await _unitOfWork.ClassSessionService.GetLastDateAsync(@class.Id);

        List<int> studentIds = request
            .Sessions
            .SelectMany(s => s.Attendances)
            .Select(a => a.StudentId)
            .ToList();

        if (@class.Studies.Any(c => !studentIds.Contains(c.Id)))
            throw new NotFoundException("Student not found");

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext
            .ClassTimeSheets
            .Where(c => c.Date == request.Date && c.ClassId == request.ClassId)
            .ToListAsync(cancellationToken: cancellationToken);

        if (classTimeSheets.Any())
            _spaceDbContext.ClassTimeSheets.RemoveRange(classTimeSheets);

        List<ClassTimeSheet> addTimeSheets = new();
        foreach (UpdateAttendanceCategorySessionDto session in request.Sessions)
        {
            ClassSession? classSession = classSessions
                .Where(cs => cs.Category == session.Category)
                .FirstOrDefault();
            if (classSession is null)
                continue;

            if (session.AttendancesWorkers.Any(c => c.TotalMinutes >= 60))
                throw new ValidationException("It cannot be more than 60 minutes");

            if (session.AttendancesWorkers.Any(c => c.TotalHours > classSession.TotalHours))
                throw new ValidationException(
                    "The worker's time cannot be longer than the lesson's time"
                );

            if (session.Attendances.Any(c => c.TotalAttendanceHours > classSession.TotalHours))
                throw new ValidationException(
                    "The student's time cannot be longer than the lesson's time"
                );

            if (classSession.Status != ClassSessionStatus.Cancelled)
            {
                ClassTimeSheet classTimeSheet =
                    new()
                    {
                        Attendances = session
                            .Attendances
                            .Select(
                                c =>
                                    new Attendance()
                                    {
                                        StudyId = c.StudentId,
                                        Note = c.Note,
                                        Status =
                                            classSession.TotalHours == c.TotalAttendanceHours
                                                ? StudentStatus.Attended
                                                : c.TotalAttendanceHours == 0
                                                    ? StudentStatus.Absent
                                                    : StudentStatus.Partial,
                                        TotalAttendanceHours = c.TotalAttendanceHours
                                    }
                            )
                            .ToList(),
                        AttendancesWorkers = session
                            .AttendancesWorkers
                            .Select(
                                wa =>
                                    new AttendanceWorker()
                                    {
                                        WorkerId = wa.WorkerId,
                                        TotalHours = wa.TotalHours,
                                        TotalMinutes = wa.TotalMinutes,
                                        RoleId = wa.RoleId,
                                        AttendanceStatus = wa.AttendanceStatus,
                                    }
                            )
                            .ToList(),
                        TotalHours = classSession.TotalHours,
                        Category = classSession.Category,
                        ClassId = classSession.ClassId,
                        EndTime = classSession.EndTime,
                        Date = classSession.Date,
                        StartTime = classSession.StartTime,
                        Status = classSession.Status,
                    };
                if (session.Category == ClassSessionCategory.Theoric && request.HeldModules != null)
                    classTimeSheet.HeldModules = request
                        .HeldModules
                        .Select(
                            hm => new HeldModule() { ModuleId = hm.ModeuleId, TotalHours = hm.TotalHours, }
                        )
                        .ToList();
                addTimeSheets.Add(classTimeSheet);
            }
            else
            {
                session.Status = ClassSessionStatus.Cancelled;

                DateOnly date2 = classLastDate.AddDays(1);
                DayOfWeek startDateDayOfWeek = date2.DayOfWeek;
                while (
                    !@class.Session.Details.Select(c => c.DayOfWeek).Any(c => date2.DayOfWeek == c)
                )
                {
                    date2 = date2.AddDays(1);
                    startDateDayOfWeek = date2.DayOfWeek;
                }
                List<ClassSession> generateClassSessions = _unitOfWork
                    .ClassSessionService
                    .GenerateSessions(
                        classSession.TotalHours,
                        request
                            .Sessions
                            .Select(
                                r =>
                                    new CreateClassSessionDto()
                                    {
                                        Category = r.Category,
                                        DayOfWeek = startDateDayOfWeek,
                                        End = classSession.EndTime,
                                        Start = classSession.StartTime
                                    }
                            )
                            .ToList(),
                        date2,
                        holidayDates,
                        @class.Id,
                        classSession.RoomId!.Value
                    );

                await _spaceDbContext
                    .ClassSessions
                    .AddRangeAsync(generateClassSessions, cancellationToken);
            }
        }
        _spaceDbContext.ClassTimeSheets.AddRange(addTimeSheets);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
