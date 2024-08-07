
namespace Space.Application.Handlers;

public class InsertAttendanceCommand : IRequest
{
    public int ClassTimeSheetId { get; set; }
    public ICollection<UpdateAttendanceDto> Attendances { get; set; } = null!;
}
internal class InsertAttendanceCommandHandler : IRequestHandler<InsertAttendanceCommand>
{
    private readonly ISpaceDbContext _spaceDbContext;

    public InsertAttendanceCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(InsertAttendanceCommand request, CancellationToken cancellationToken)
    {
        ClassTimeSheet? classTimeSheet = _spaceDbContext.ClassTimeSheets
            .Where(c => c.Id == request.ClassTimeSheetId)
            .Include(c => c.Attendances)
            .FirstOrDefault()
                ?? throw new NotFoundException(nameof(ClassTimeSheet), request.ClassTimeSheetId);

        classTimeSheet.Attendances = request.Attendances.Select(a => new Attendance()
        {
            ClassTimeSheetsId = classTimeSheet.Id,
            StudyId = a.StudentId,
            TotalAttendanceHours = a.TotalAttendanceHours,
            Note = a.Note,
        }).ToList();

        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}