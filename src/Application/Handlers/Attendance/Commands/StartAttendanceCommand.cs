
namespace Space.Application.Handlers;

public class StartAttendanceCommand : IRequest<GetClassTimeSheetResponseDto>
{
    public int ClassId { get; set; }
    public ClassSessionCategory SessionCategory { get; set; }
    public ICollection<int>? HeldModulesIds { get; set; }
}
internal class StartAttendanceCommandHandler : IRequestHandler<StartAttendanceCommand, GetClassTimeSheetResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly ICurrentUserService _currentUserService;
    public StartAttendanceCommandHandler(
        ISpaceDbContext spaceDbContext,
        ICurrentUserService currentUserService)
    {
        _spaceDbContext = spaceDbContext;
        _currentUserService = currentUserService;
    }
    public async Task<GetClassTimeSheetResponseDto> Handle(StartAttendanceCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId == null) throw new UnauthorizedAccessException();

        Worker worker = await _spaceDbContext.Workers.FindAsync(_currentUserService.UserId)
            ?? throw new NotFoundException(nameof(Worker), _currentUserService.UserId);

        DateTime nowDate = DateTime.Now;
        DateOnly now = DateOnly.FromDateTime(nowDate);
        TimeOnly nowTime = TimeOnly.FromDateTime(nowDate);

        Class? classEntity = await _spaceDbContext.Classes.FindAsync(request.ClassId)
                    ?? throw new NotFoundException(nameof(Class), request.ClassId);

        ClassTimeSheet? classTimeSheet = await _spaceDbContext.ClassTimeSheets
                    .Where(c => c.ClassId == request.ClassId && c.Date == now)
                    .FirstOrDefaultAsync();
        if (classTimeSheet != null) throw new BadHttpRequestException("Attendance already started for this class");

        classTimeSheet = new ClassTimeSheet()
        {
            ClassId = request.ClassId,
            Date = now,
            StartTime = nowTime,
            TotalHours = 0,
            Status = ClassSessionStatus.Offline,
            Category = request.SessionCategory,
        };

        if (request.HeldModulesIds != null && request.HeldModulesIds.Count != 0)
        {
            List<int> requestModuleIds = request.HeldModulesIds.Select(c => c).ToList();

            List<Module> module = await _spaceDbContext.Modules
                .Where(m => requestModuleIds.Contains(m.Id))
                .ToListAsync(cancellationToken: cancellationToken);
            if (requestModuleIds.Count != module.Count) throw new NotFoundException("Modules not found");

            classTimeSheet.HeldModules = request
                                             .HeldModulesIds
                                             .Select(
                                                 hm => new HeldModule() { ModuleId = hm, TotalHours = 0, }
                                             )
                                             .ToList();
        }



        AttendanceWorker attendanceWorker = new()
        {
            WorkerId = worker.Id,
            StartTime = nowTime,
            TotalHours = 0,
            ClassTimeSheet = classTimeSheet,
        };

        classTimeSheet.AttendancesWorkers.Add(attendanceWorker);
        await _spaceDbContext.ClassTimeSheets.AddAsync(classTimeSheet);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);

        return new GetClassTimeSheetResponseDto()
        {
            ClassTimeSheetId = classTimeSheet.Id,
            StartTime = classTimeSheet.StartTime,
            Category = classTimeSheet.Category,
            Class = new GetClassDto()
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
            },
            EndTime = classTimeSheet.EndTime,
        };
    }
}