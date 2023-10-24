namespace Space.Application.Handlers;

public class CreateClassSessionByClassCommand : IRequest
{
    public Guid Id { get; set; }
    public CreateClassSessionByClassRequestDto Session { get; set; }
}
internal class CreateClassSessionByClassCommandHandler : IRequestHandler<CreateClassSessionByClassCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IClassRepository _classRepository;
    readonly IRoomRepository _roomRepository;

    public CreateClassSessionByClassCommandHandler(
        IUnitOfWork unitOfWork,
        IClassRepository classRepository,
        IRoomRepository roomRepository)
    {
        _unitOfWork = unitOfWork;
        _classRepository = classRepository;
        _roomRepository = roomRepository;
    }

    public async Task Handle(CreateClassSessionByClassCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _classRepository.GetAsync(request.Id, true, "ClassSessions") ??
            throw new NotFoundException(nameof(Class), request.Id);

        Room room = await _roomRepository.GetAsync(request.Session.RoomId) ??
            throw new NotFoundException(nameof(Room), request.Session.RoomId);

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
            Date = new DateTime(request.Session.Date.Day, request.Session.Date.Month, request.Session.Date.Day),
            RoomId = room.Id,
            TotalHour = (request.Session.End - request.Session.Start).Hours,
            Category = request.Session.Category
        });
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
