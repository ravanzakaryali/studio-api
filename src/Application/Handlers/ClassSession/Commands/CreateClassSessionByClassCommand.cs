namespace Space.Application.Handlers;

public class CreateClassSessionByClassCommand : IRequest
{
    public int Id { get; set; }
    public CreateClassSessionByClassRequestDto Session { get; set; } = null!;
}
internal class CreateClassSessionByClassCommandHandler : IRequestHandler<CreateClassSessionByClassCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    public CreateClassSessionByClassCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateClassSessionByClassCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Include("ClassSessions")
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        if (@class.ClassSessions.Any(c =>
                                    (c.StartTime >= request.Session.Start && c.StartTime < request.Session.Start) ||
                                    (c.EndTime > request.Session.End && c.StartTime <= request.Session.End) &&
                                    (new DateOnly(c.Date.Year, c.Date.Month, c.Date.Day) == request.Session.Date)))
        {
            throw new DateTimeException("There is already a lesson in this date and time range");
        }

        @class.ClassSessions.Add(new ClassSession()
        {
            StartTime = request.Session.Start,
            EndTime = request.Session.End,
            Date = request.Session.Date,
            RoomId = @class.RoomId,
            TotalHours = (request.Session.End - request.Session.Start).Hours,
            Category = request.Session.Category
        });
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
