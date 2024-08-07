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
                .Include(c => c.Session)
                .ThenInclude(c => c.Details)
                .Include(c => c.Studies)
                .Include(c => c.Program)
                .ThenInclude(c => c.Modules)
                .ThenInclude(c => c.SubModules)
                .Include(c => c.ClassModulesWorkers)
                .ThenInclude(c => c.Worker)
                .Include(c => c.ClassModulesWorkers)
                .ThenInclude(c => c.Role)
                .Include(c => c.Session)
                .ThenInclude(c => c.Details)
                .Where(c => c.Id == request.ClassId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Class), request.ClassId);


        DateOnly dateNow = request.Date;
        if (request.HeldModules != null && request.HeldModules!.Count != 0)
        {
            List<int> requestModuleIds = request.HeldModules.Select(c => c.ModuleId).ToList();
            List<Module> module = await _spaceDbContext
                .Modules
                .Where(m => requestModuleIds.Contains(m.Id))
                .ToListAsync(cancellationToken: cancellationToken);

            if (requestModuleIds.Count != module.Count)
                throw new NotFoundException("Modules not found");
        }

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
            .Where(c => c.Date == dateNow && c.ClassId == request.ClassId)
            .ToListAsync(cancellationToken: cancellationToken);

        if (classTimeSheets.Any())
            _spaceDbContext.ClassTimeSheets.RemoveRange(classTimeSheets);

        List<ClassTimeSheet> addTimeSheets = new();
        foreach (UpdateAttendanceCategorySessionDto session in request.Sessions)
        {
            SessionDetail sessionDetail = @class.Session.Details.FirstOrDefault(c => c.DayOfWeek == dateNow.DayOfWeek)
                ?? throw new NotFoundException("Session detail not found");

            if (session.AttendancesWorkers.Any(c => c.TotalMinutes >= 60))
                throw new ValidationException("It cannot be more than 60 minutes");

            if (session.AttendancesWorkers.Any(c => c.TotalHours > sessionDetail.TotalHours))
                throw new ValidationException(
                    "The worker's time cannot be longer than the lesson's time"
                );

            if (session.Attendances.Any(c => c.TotalAttendanceHours > sessionDetail.TotalHours))
                throw new ValidationException(
                    "The student's time cannot be longer than the lesson's time"
                );

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
                                        sessionDetail.TotalHours == c.TotalAttendanceHours
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
                                }
                        )
                        .ToList(),
                    TotalHours = sessionDetail.TotalHours,
                    Category = session.Category,
                    ClassId = @class.Id,
                    EndTime = sessionDetail.EndTime,
                    Date = dateNow,
                    StartTime = sessionDetail.StartTime,
                    Status = session.Status,
                };
            if (session.Category == ClassSessionCategory.Theoric && request.HeldModules != null)
            {
                classTimeSheet.HeldModules = request
                                        .HeldModules
                                        .Select(
                                            hm => new HeldModule() { ModuleId = hm.ModuleId, TotalHours = hm.TotalHours, }
                                        )
                                        .ToList();
            }

            addTimeSheets.Add(classTimeSheet);
        }
        _spaceDbContext.ClassTimeSheets.AddRange(addTimeSheets);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
