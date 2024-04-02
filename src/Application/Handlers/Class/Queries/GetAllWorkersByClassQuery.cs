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
            .Include(c => c.ClassSessions)
            .Include(c => c.ClassModulesWorkers)
                .ThenInclude(c => c.Worker)
            .Include(c => c.ClassModulesWorkers)
                .ThenInclude(c => c.Role)
            .Include(c => c.ClassModulesWorkers)
                .ThenInclude(c => c.Module)
            .Include(c => c.ClassExtraModulesWorkers)
                .ThenInclude(c => c.Worker)
            .Include(c => c.ClassExtraModulesWorkers)
                .ThenInclude(c => c.Role)
            .Include(c => c.ClassExtraModulesWorkers)
                .ThenInclude(c => c.ExtraModule)
            .Include(c => c.ClassTimeSheets)
                .ThenInclude(c => c.AttendancesWorkers)
                .ThenInclude(c => c.Worker)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Class), request.Id);


        DateOnly requestDate = DateOnly.FromDateTime(request.Date);

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == @class.Id && requestDate >= c.Date)
            .ToListAsync(cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(ClassTimeSheet), request.Id);


        List<GetWorkersByClassResponseDto> workers = new();


        foreach (ClassTimeSheet classTimeSheet in classTimeSheets.Where(cts => cts.Date == requestDate))
        {
            foreach (AttendanceWorker attendance in classTimeSheet.AttendancesWorkers)
            {
                workers.Add(new GetWorkersByClassResponseDto()
                {
                    Name = attendance.Worker.Name!,
                    Surname = attendance.Worker.Surname!,
                    RoleId = attendance.RoleId,
                    RoleName = attendance.Role!.Name,
                    TotalHours = attendance.TotalHours,
                    TotalMinutes = attendance.TotalMinutes,
                    WorkerId = attendance.WorkerId,
                    AttendanceStatus = attendance.AttendanceStatus,
                    TotalLessonHours = @class.ClassTimeSheets
                        .Where(session => session.Status == ClassSessionStatus.Offline || session.Status == ClassSessionStatus.Online)
                        .SelectMany(c => c.AttendancesWorkers)
                        .Where(attendance => attendance.WorkerId == attendance.WorkerId)
                        .Sum(c => c.TotalHours)
                });
            }
        }
        if (!workers.Any())
        {
            if (@class.ClassModulesWorkers.Any(c => c.StartDate <= requestDate && c.EndDate >= requestDate))
            {
                workers.AddRange(@class.ClassModulesWorkers
                                   .Where(c => c.StartDate <= requestDate && c.EndDate >= requestDate && c.Module.TopModuleId != null)
                                   .Distinct(new GetWorkerForClassDtoComparer())
                                   .OrderBy(c => c.StartDate)
                                   .Take(2)
                                   .Select(c =>
                                   {
                                       List<ClassTimeSheet> classTimeSheets = @class.ClassTimeSheets.Where(cts => cts.Date == requestDate).ToList();
                                       GetWorkersByClassResponseDto workersClass = new()
                                       {
                                           Name = c.Worker.Name!,
                                           Surname = c.Worker.Surname!,
                                           RoleId = c.RoleId,
                                           RoleName = c.Role!.Name,
                                           WorkerId = c.WorkerId,
                                           TotalLessonHours = @class.ClassTimeSheets
                                               .Where(session => session.Status == ClassSessionStatus.Offline || session.Status == ClassSessionStatus.Online)
                                               .SelectMany(c => c.AttendancesWorkers)
                                               .Where(attendance => attendance.WorkerId == c.WorkerId)
                                               .Sum(c => c.TotalHours)
                                       };

                                       return workersClass;
                                   }));
            }
            else
            {
                workers.AddRange(@class.ClassExtraModulesWorkers
                                   .Where(c => c.StartDate <= requestDate && c.EndDate >= requestDate)
                                   .Distinct(new GetWorkerForClassDtoExtraModuleComparer())
                                   .OrderBy(c => c.StartDate)
                                   .Take(2)
                                   .Select(c =>
                                   {
                                       List<ClassTimeSheet> classTimeSheets = @class.ClassTimeSheets.Where(cts => cts.Date == requestDate).ToList();
                                       GetWorkersByClassResponseDto workersClass = new()
                                       {
                                           Name = c.Worker.Name!,
                                           Surname = c.Worker.Surname!,
                                           RoleId = c.RoleId,
                                           RoleName = c.Role!.Name,
                                           WorkerId = c.WorkerId,
                                           TotalLessonHours = @class.ClassTimeSheets
                                               .Where(session => session.Status == ClassSessionStatus.Offline || session.Status == ClassSessionStatus.Online)
                                               .SelectMany(c => c.AttendancesWorkers)
                                               .Where(attendance => attendance.WorkerId == c.WorkerId)
                                               .Sum(c => c.TotalHours)
                                       };

                                       return workersClass;
                                   }));
            }


        }


        return workers;
    }
}