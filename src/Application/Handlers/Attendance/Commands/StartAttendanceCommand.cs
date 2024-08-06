
namespace Space.Application.Handlers;

public class StartAttendanceCommand : IRequest
{
    public StartAttendanceCommand()
    {
        ModulesId = new HashSet<int>();
    }
    public int ClassId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public ClassSessionCategory SessionCategory { get; set; }
    public ICollection<int> ModulesId { get; set; }
    public int WorkerId { get; set; }
}
internal class StartAttendanceCommandHandler : IRequestHandler<StartAttendanceCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    public StartAttendanceCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task Handle(StartAttendanceCommand request, CancellationToken cancellationToken)
    {
        Class? classEntity = await _spaceDbContext.Classes.FindAsync(request.ClassId)
                    ?? throw new NotFoundException(nameof(Class), request.ClassId);

        ClassTimeSheet? classTimeSheet = await _spaceDbContext.ClassTimeSheets
                    .Where(c => c.ClassId == request.ClassId && c.Date == request.Date)
                    .FirstOrDefaultAsync();
        if (classTimeSheet != null) throw new BadHttpRequestException("Attendance already started for this class");

        throw new NotImplementedException();

    }
}