namespace Space.Application.Handlers;

public record GetAllWorkersByClassQuery(Guid Id, DateOnly Date) : IRequest<IEnumerable<GetWorkersByClassResponseDto>>;

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
            .FirstOrDefaultAsync()
                ?? throw new NotFoundException(nameof(Class), request.Id);

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == @class.Id && c.Category != ClassSessionCategory.Lab && request.Date >= c.Date)
            .ToListAsync()
                ?? throw new NotFoundException(nameof(ClassTimeSheet), request.Id);

        return @class.ClassModulesWorkers.Where(c => c.StartDate >= request.Date && c.EndDate <= request.Date).Distinct(new GetWorkerForClassDtoComparer()).Select(c =>
        {
            List<ClassTimeSheet> classTimeSheets = @class.ClassTimeSheets.Where(c => c.Date == request.Date).ToList();
            bool isAttendance = false;
            foreach (ClassTimeSheet classTimeSheet in classTimeSheets)
            {
                AttendanceWorker? attendance = classTimeSheet.AttendancesWorkers.FirstOrDefault(attendance => attendance.WorkerId == c.WorkerId);
                if (attendance != null)
                {
                    isAttendance = attendance.TotalAttendanceHours == classTimeSheet.TotalHours;
                }
            }
            return new GetWorkersByClassResponseDto()
            {
                Name = c.Worker.Name!,
                Surname = c.Worker.Surname!,
                RoleId = c.RoleId,
                RoleName = c.Role!.Name,
                WorkerId = c.WorkerId,
                IsAttendance = isAttendance,
                TotalLessonHours = @class.ClassSessions.Where(session => session.Status == ClassSessionStatus.Offline || session.Status == ClassSessionStatus.Online).SelectMany(c => c.AttendancesWorkers).Where(attendance => attendance.WorkerId == c.WorkerId).Sum(c => c.TotalAttendanceHours)
            };
        });
    }
}