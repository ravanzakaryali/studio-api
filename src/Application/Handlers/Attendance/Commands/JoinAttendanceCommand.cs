namespace Space.Application.Handlers;

public class JoinAttendanceCommand : IRequest<GetClassTimeSheetResponseDto>
{
    public int ClassTimeSheetId { get; set; }
}

internal class JoinAttendanceCommandHandler : IRequestHandler<JoinAttendanceCommand, GetClassTimeSheetResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly ICurrentUserService _currentUserService;
    public JoinAttendanceCommandHandler(
        ISpaceDbContext spaceDbContext,
        ICurrentUserService currentUserService)
    {
        _spaceDbContext = spaceDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<GetClassTimeSheetResponseDto> Handle(JoinAttendanceCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId == null) throw new UnauthorizedAccessException();

        Worker? worker = await _spaceDbContext.Workers.Where(c => c.Id == int.Parse(_currentUserService.UserId))
                    .FirstOrDefaultAsync()
            ?? throw new NotFoundException(nameof(Worker), _currentUserService.UserId);

        DateTime nowDate = DateTime.Now;
        DateOnly now = DateOnly.FromDateTime(nowDate);
        TimeOnly nowTime = TimeOnly.FromDateTime(nowDate);

        ClassTimeSheet? classTimeSheet = await _spaceDbContext.ClassTimeSheets
                    .Where(c => c.Id == request.ClassTimeSheetId)
                    .Include(c => c.Class)
                    .Include(c => c.AttendancesWorkers)
                    .FirstOrDefaultAsync()
                    ?? throw new NotFoundException(nameof(ClassTimeSheet), request.ClassTimeSheetId);

        if (classTimeSheet.AttendancesWorkers.Any(a => a.WorkerId == worker.Id)) throw new BadHttpRequestException("Attendance already started for this class");

        classTimeSheet.AttendancesWorkers.Add(new AttendanceWorker()
        {
            WorkerId = worker.Id,
            StartTime = nowTime,
            TotalHours = 0,
            TotalMinutes = 0,
        });

        if (classTimeSheet == null) throw new NotFoundException(nameof(ClassTimeSheet), request.ClassTimeSheetId);

        return new GetClassTimeSheetResponseDto()
        {
            ClassTimeSheetId = classTimeSheet.Id,
            StartTime = classTimeSheet.StartTime,
            Category = classTimeSheet.Category,
            IsJoined = classTimeSheet.AttendancesWorkers.Any(a => a.WorkerId == worker.Id && a.StartTime == nowTime),
            Class = new GetClassDto()
            {
                Id = classTimeSheet.Class.Id,
                Name = classTimeSheet.Class.Name,
            },
            EndTime = classTimeSheet.EndTime,
        };


    }
}