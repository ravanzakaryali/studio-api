namespace Space.Application.Handlers;

public class GetAllWorkersByClassQuery : IRequest<IEnumerable<GetWorkersByClassResponseDto>>
{
    public GetAllWorkersByClassQuery(int id)
    {
        Id = id;
        Date = DateTime.Now.Date;
    }
    public GetAllWorkersByClassQuery(int id, DateTime date)
    {
        Id = id;
        Date = date;
    }
    public int Id { get; set; }
    public DateTime Date { get; set; }
}

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
                var mentor = @class.ClassModulesWorkers
                .FirstOrDefault(c => c.StartDate <= requestDate && c.EndDate >= requestDate && c.Module.TopModuleId != null &&
                        c.Role.Name == "mentor");

                if (mentor != null)
                {
                    workers.Add(new GetWorkersByClassResponseDto
                    {
                        Name = mentor.Worker.Name!,
                        Surname = mentor.Worker.Surname!,
                        RoleId = mentor.RoleId,
                        RoleName = mentor.Role!.Name,
                        WorkerId = mentor.WorkerId,
                        TotalLessonHours = @class.ClassTimeSheets
                            .Where(session => session.Status == ClassSessionStatus.Offline || session.Status == ClassSessionStatus.Online)
                            .SelectMany(c => c.AttendancesWorkers)
                            .Where(attendance => attendance.WorkerId == mentor.WorkerId)
                            .Sum(c => c.TotalHours)
                    });
                }

                var muellim = @class.ClassModulesWorkers
                    .FirstOrDefault(c => c.StartDate <= requestDate && c.EndDate >= requestDate && c.Module.TopModuleId != null &&
                                c.Role.Name == "muellim");

                if (muellim != null)
                {
                    workers.Add(new GetWorkersByClassResponseDto
                    {
                        Name = muellim.Worker.Name!,
                        Surname = muellim.Worker.Surname!,
                        RoleId = muellim.RoleId,
                        RoleName = muellim.Role!.Name,
                        WorkerId = muellim.WorkerId,
                        TotalLessonHours = @class.ClassTimeSheets
                            .Where(session => session.Status == ClassSessionStatus.Offline || session.Status == ClassSessionStatus.Online)
                            .SelectMany(c => c.AttendancesWorkers)
                            .Where(attendance => attendance.WorkerId == muellim.WorkerId)
                            .Sum(c => c.TotalHours)
                    });
                }
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
                                           Email = c.Worker.Email,
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