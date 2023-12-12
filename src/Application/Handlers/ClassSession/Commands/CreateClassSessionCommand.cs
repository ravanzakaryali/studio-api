namespace Space.Application.Handlers;

public class CreateClassSessionCommand : IRequest
{
    public int ClassId { get; set; }
    public int SessionId { get; set; }
}

internal class CreateClassSessionCommandHandler : IRequestHandler<CreateClassSessionCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateClassSessionCommandHandler(
        IUnitOfWork unitOfWork,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateClassSessionCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Include(c => c.Session)
            .ThenInclude(c => c.Details)
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .ThenInclude(c => c.SubModules)
            .Where(c => c.Id == request.ClassId)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.ClassId);
        _spaceDbContext.ClassSessions.RemoveRange(await _spaceDbContext.ClassSessions.Where(cr => cr.ClassId == @class.Id).ToListAsync());

        Session session = await _spaceDbContext.Sessions
            .Include(c => c.Details)
            .Where(c => c.Id == request.SessionId)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Session), request.SessionId);

        if (@class.RoomId == null)
            throw new Exception("Class room null");

        List<CreateClassSessionDto> sessions = session.Details
            .Select(c => new CreateClassSessionDto()
            {
                Category = c.Category,
                DayOfWeek = c.DayOfWeek,
                End = c.EndTime,
                Start = c.StartTime
            }).ToList();

        List<DayOfWeek> selectedDays = sessions.Select(c => c.DayOfWeek).ToList();

        List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();


        List<ClassSession> classSessions = _unitOfWork.ClassSessionService.GenerateSessions(
                                                                                       @class.Program.TotalHours,
                                                                                       sessions,
                                                                                       @class.StartDate,
                                                                                       holidayDates,
                                                                                       @class.Id,
                                                                                       @class.RoomId.Value);

        @class.EndDate = classSessions.Max(c => c.Date);
        await _spaceDbContext.ClassSessions.AddRangeAsync(classSessions);

    }
}