namespace Space.Application.Handlers;

public record GetAllWorkersByClassQuery(int Id, DateTime Date) : IRequest<IEnumerable<GetWorkersByClassResponseDto>>;

internal class GetAllWorkersByClassQueryHandler : IRequestHandler<GetAllWorkersByClassQuery, IEnumerable<GetWorkersByClassResponseDto>>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllWorkersByClassQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetWorkersByClassResponseDto>> Handle(GetAllWorkersByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Worker)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Role)
            .Include(c => c.ClassTimeSheets)
            .ThenInclude(c => c.AttendancesWorkers)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Class), request.Id);


        DateOnly requestDate = DateOnly.FromDateTime(request.Date);

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == @class.Id && c.Category != ClassSessionCategory.Lab && requestDate >= c.Date)
            .ToListAsync(cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(ClassTimeSheet), request.Id);



        return @class.ClassModulesWorkers
            .Where(c => c.StartDate >= requestDate && c.EndDate <= requestDate)
            .Distinct(new GetWorkerForClassDtoComparer())
            .Select(c =>
            {
                List<ClassTimeSheet> classTimeSheets = @class.ClassTimeSheets.Where(c => c.Date == requestDate).ToList();
                int? totalHours = 0;
                int? totalMinutes = 0;
                AttendanceStatus? attendanceStatus = null;
                foreach (ClassTimeSheet classTimeSheet in classTimeSheets)
                {
                    AttendanceWorker? attendance = classTimeSheet.AttendancesWorkers.FirstOrDefault(attendance => attendance.WorkerId == c.WorkerId);
                    totalHours = attendance?.TotalHours;
                    totalMinutes = attendance?.TotalMinutes;
                    attendanceStatus = attendance?.AttendanceStatus;
                }
                return new GetWorkersByClassResponseDto()
                {
                    Name = c.Worker.Name!,
                    AttendanceStatus = attendanceStatus,
                    Surname = c.Worker.Surname!,
                    RoleId = c.RoleId,
                    RoleName = c.Role!.Name,
                    WorkerId = c.WorkerId,
                    TotalHours = totalHours,
                    TotalMinutes = totalMinutes,
                    TotalLessonHours = @class.ClassTimeSheets.Where(session => session.Status == ClassSessionStatus.Offline || session.Status == ClassSessionStatus.Online).SelectMany(c => c.AttendancesWorkers).Where(attendance => attendance.WorkerId == c.WorkerId).Sum(c => c.TotalHours)
                };
            });
    }
}