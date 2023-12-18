namespace Space.Application.Handlers;

public class UpdateClassSessionByDateCommand : IRequest
{
    public int ClassId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}

internal class UpdateClassSessionByDateCommandHandler : IRequestHandler<UpdateClassSessionByDateCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;

    public UpdateClassSessionByDateCommandHandler(
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork)
    {
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateClassSessionByDateCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes.FirstOrDefaultAsync(c => c.Id == request.ClassId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
                                                                .Include(c => c.ClassTimeSheet)
                                                                .Where(c => c.ClassId == request.ClassId &&
                                                                            c.Date >= request.StartDate &&
                                                                            c.Date <= request.EndDate)
                                                                .ToListAsync(cancellationToken: cancellationToken);

        //Todo: Code review
        if (classSessions.Any(c =>
                c.ClassTimeSheet?.Status == ClassSessionStatus.Offline &&
                c.ClassTimeSheet?.Status == ClassSessionStatus.Online))
            throw new Exception("Seçilən tarix daxilində qeyd olunmuş sessiya var!");

        if (@class.RoomId is null) throw new NotFoundException("Bu qrup hər hansı bir dərs otağına əlavə olunmayıb");


        List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();

        List<ClassSession> responseClassSessions = _unitOfWork.ClassSessionService.GenerateSessions(
            startDate: request.StartDate, request.Sessions.ToList(), request.EndDate, holidayDates, @class.Id, @class.RoomId.Value);

        await _spaceDbContext.ClassSessions.AddRangeAsync(responseClassSessions);
        _spaceDbContext.ClassSessions.RemoveRange(classSessions);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
