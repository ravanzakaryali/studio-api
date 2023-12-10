namespace Space.Application.Handlers;

public class GetWorkerByClassQuery : IRequest<GetWorkersByClassResponseDto>
{
    public Guid ClassId { get; set; }
    public Guid WorkerId { get; set; }
    public DateOnly Date { get; set; }
    public Guid RoleId { get; set; }
}

internal class GetWorkerByClassQueryHandler : IRequestHandler<GetWorkerByClassQuery, GetWorkersByClassResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetWorkerByClassQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetWorkersByClassResponseDto> Handle(GetWorkerByClassQuery request, CancellationToken cancellationToken)
    {

        Worker? worker = await _spaceDbContext.Workers.FirstOrDefaultAsync(w => w.Id == request.WorkerId)
            ?? throw new NotFoundException(nameof(Worker), request.WorkerId);

        Class? @class = await _spaceDbContext.Classes
            .Include(c => c.ClassTimeSheets)
            .ThenInclude(cs => cs.AttendancesWorkers)
            .FirstOrDefaultAsync(c => c.Id == request.ClassId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Class), request.ClassId);

        Role role = await _spaceDbContext.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Role), request.RoleId);

        int totalHour = 0;
        int? totalAttendanceHours = 0;
        int? totalAttendanceMinutes = 0;

        @class.ClassTimeSheets.ToList().ForEach(cs =>
        {
            cs.AttendancesWorkers.Where(c => c.WorkerId == request.WorkerId).ToList().ForEach(aw =>
            {
                totalHour += aw.TotalHours;
            });
            if (cs.Date == request.Date)
            {
                totalAttendanceHours = cs.AttendancesWorkers.FirstOrDefault(c => c.WorkerId == request.WorkerId)?.TotalHours;
                totalAttendanceMinutes = cs.AttendancesWorkers.FirstOrDefault(c => c.WorkerId == request.WorkerId)?.TotalMinutes;
            }
        });

        return new GetWorkersByClassResponseDto()
        {
            TotalHours = totalAttendanceHours,
            TotalMinutes = totalAttendanceMinutes,
            Name = worker.Name!,
            Surname = worker.Surname!,
            TotalLessonHours = totalHour,
            RoleId = role.Id,
            RoleName = role.Name,
            WorkerId = request.WorkerId,
        };
    }
}
