using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Space.Application.Handlers.Commands;

public record CancelledAttendanceCommand(
        int ClassId, DateTime Date) : IRequest;
internal class CancelledAttendanceHandler : IRequestHandler<CancelledAttendanceCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;

    public CancelledAttendanceHandler(
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork)
    {
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelledAttendanceCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
                        .Include(c => c.Session)
                        .ThenInclude(c => c.Details)
                        .FirstOrDefaultAsync(c => c.Id == request.ClassId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        DateOnly date = DateOnly.FromDateTime(request.Date);
        if (!@class.Session.Details.Any(c => c.DayOfWeek == date.DayOfWeek))
        {
            throw new NotFoundException(nameof(ClassSession), request.ClassId);
        }

        IEnumerable<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == request.ClassId && c.Date == date)
            .Include(c => c.Attendances)
            .Include(c => c.AttendancesWorkers)
            .ToListAsync(cancellationToken: cancellationToken);

        if (!classTimeSheets.Any())
        {
            _spaceDbContext.ClassTimeSheets.Add(new ClassTimeSheet
            {
                ClassId = request.ClassId,
                Date = date,
                Status = ClassSessionStatus.Cancelled
            });
            @class.EndDate = @class.EndDate!.Value.AddDays(1);
        }
        else
        {
            foreach (ClassTimeSheet item in classTimeSheets)
            {
                item.Attendances = new List<Attendance>();
                item.Status = ClassSessionStatus.Cancelled;
                item.AttendancesWorkers = new List<AttendanceWorker>();
            }
        }

        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
