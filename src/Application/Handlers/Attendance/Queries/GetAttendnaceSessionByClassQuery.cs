
public class GetAttendnaceSessionByClassQuery : IRequest<GetAttendanceSessionDto>
{
    public int ClassId { get; set; }
    public WorkerType WorkerType { get; set; }
}
internal class GetAttendnaceSessionByClassQueryHandler : IRequestHandler<GetAttendnaceSessionByClassQuery, GetAttendanceSessionDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMediator _mediator;
    readonly ICurrentUserService _currentUserService;

    public GetAttendnaceSessionByClassQueryHandler(ISpaceDbContext spaceDbContext, ICurrentUserService currentUserService, IMediator mediator)
    {
        _spaceDbContext = spaceDbContext;
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    public async Task<GetAttendanceSessionDto> Handle(GetAttendnaceSessionByClassQuery request, CancellationToken cancellationToken)
    {
        int loginUserId = int.Parse(_currentUserService.UserId ?? throw new UnauthorizedAccessException());


        DateTime dateTimeNow = DateTime.UtcNow.AddHours(4);
        DateOnly dateNow = DateOnly.FromDateTime(dateTimeNow);

        Class? @class = await _spaceDbContext.Classes
            .Include(c => c.Session)
            .ThenInclude(s => s.Details)
            .FirstOrDefaultAsync(c => c.Id == request.ClassId, cancellationToken: cancellationToken) ??
            throw new NotFoundException(nameof(Class), request.ClassId);

        ClassSessionCategory category = ClassSessionCategory.Theoric;

        if (request.WorkerType == WorkerType.Mentor)
        {
            category = ClassSessionCategory.Lab;
        }

        ClassTimeSheet? classTimeSheets = await _spaceDbContext.ClassTimeSheets
                        .Where(c => c.ClassId == @class.Id && c.Date == dateNow && c.Category == category)
                        .Include(c => c.AttendancesWorkers)
                        .Include(c => c.HeldModules)
                        .ThenInclude(hm => hm.Module)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken);


        ClassModulesWorker? classModulesWorker = await _spaceDbContext.ClassModulesWorkers
            .Where(c => c.ClassId == @class.Id && c.StartDate <= dateNow && (c.EndDate == null || c.EndDate >= dateNow) && c.WorkerType == request.WorkerType)
            .Include(c => c.Worker)
            .Include(c => c.Role)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (classModulesWorker == null)
        {
            classModulesWorker = await _spaceDbContext.ClassModulesWorkers
            .Where(c => c.ClassId == @class.Id && c.WorkerType == request.WorkerType)
            .Include(c => c.Worker)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (classModulesWorker == null)
            {
                throw new NotFoundException(nameof(ClassModulesWorker), request.ClassId);
            }
        }

        IEnumerable<GetAllStudentByClassResponseDto> attendanceStudents = await _mediator.Send(new GetAllStudentsByClassQuery(request.ClassId, dateTimeNow));

        GetAttendanceSessionDto response = new()
        {
            Class = new GetClassAttendanceDto()
            {
                Id = @class.Id,
                Name = @class.Name,
            },
            TotalHours = @class.Session.Details.FirstOrDefault(d => d.DayOfWeek == dateNow.DayOfWeek)?.TotalHours ?? 0,
            Students = attendanceStudents,

            Worker = new UserDto()
            {
                Id = classModulesWorker.Worker.Id,
                Name = classModulesWorker.Worker.Name,
                Surname = classModulesWorker.Worker.Surname,
                Email = classModulesWorker.Worker.Email,
                WorkerType = classModulesWorker.WorkerType,
            }
        };

        if (classTimeSheets != null)
        {
            response.ClassTimeSheetId = classTimeSheets.Id;
            response.StartTime = classTimeSheets.StartTime;
            response.EndTime = classTimeSheets.EndTime;
            response.IsJoined = classTimeSheets.AttendancesWorkers.Any(a => a.WorkerId == loginUserId);
            response.Category = classTimeSheets.Category;
            response.HeldModules = classTimeSheets.HeldModules.Select(hm => new GetHeldModulesDto()
            {
                Id = hm.Id,
                Name = hm.Module?.Name ?? "",
                TotalHours = hm.TotalHours,
                Version = hm.Module?.Version,
            });
        }

        return response;

    }
}
