using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Space.Application.Handlers.Commands;

public record CancelledAttendanceCommand(
        int ClassId, DateTime Date) : IRequest;
internal class CancelledAttendanceHandler : IRequestHandler<CancelledAttendanceCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public CancelledAttendanceHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CancelledAttendanceCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes.FindAsync(request.ClassId)
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        DateOnly date = DateOnly.FromDateTime(request.Date);

        IEnumerable<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.ClassId == request.ClassId && c.Date == date)
            .ToListAsync(cancellationToken: cancellationToken);

        if (!classSessions.Any())
        {
            throw new NotFoundException(nameof(ClassSession), request.ClassId);
        }
        foreach (ClassSession classSession in classSessions)
        {
            classSession.Status = ClassSessionStatus.Cancelled;
        }
        IEnumerable<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == request.ClassId && c.Date == date)
            .ToListAsync(cancellationToken: cancellationToken);

        _spaceDbContext.ClassTimeSheets.RemoveRange(classTimeSheets);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
