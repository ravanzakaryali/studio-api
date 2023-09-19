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
            .Include(c => c.ClassSessions)
            .ThenInclude(cs => cs.AttendancesWorkers)
            .FirstOrDefaultAsync(c => c.Id == request.ClassId, cancellationToken)
                    ?? throw new NotFoundException(nameof(Class), request.ClassId);

        Role role = await _spaceDbContext.Roles.FirstOrDefaultAsync(r => r.Id == request.RoleId)
            ?? throw new NotFoundException(nameof(Role), request.RoleId);

        int totalHour = 0;
        bool isAttendance = false;

        @class.ClassSessions.ForEach(cs =>
        {
            cs.AttendancesWorkers.Where(c => c.WorkerId == request.WorkerId).ToList().ForEach(aw =>
            {
                totalHour += aw.TotalAttendanceHours;
            });
            if (cs.Date == new DateTime(request.Date.Year, request.Date.Month, request.Date.Day))
            {
                isAttendance = cs.AttendancesWorkers.Any(c => c.WorkerId == request.WorkerId && c.TotalAttendanceHours != 0);
            }
        });

        return new GetWorkersByClassResponseDto()
        {
            IsAttendance = isAttendance,
            Name = worker.Name!,
            Surname = worker.Surname!,
            TotalLessonHours = totalHour,
            WorkerId = request.WorkerId,
        };
    }
}
