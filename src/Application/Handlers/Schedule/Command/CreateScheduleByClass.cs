namespace Space.Application.Handlers;

public record CreateRoomScheduleByClassCommand : IRequest;

internal class CreateRoomScheduleByClassCommandHandler : IRequestHandler<CreateRoomScheduleByClassCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public CreateRoomScheduleByClassCommandHandler(
                                ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateRoomScheduleByClassCommand request, CancellationToken cancellationToken)
    {

        IEnumerable<Class> allClasses = await _spaceDbContext.Classes
            .Include(c => c.ClassSessions)
            .Include(c => c.RoomSchedules)
            .ToListAsync();

        foreach (Class @class in allClasses)
        {
            RoomSchedule? scheduleControl = @class.RoomSchedules.FirstOrDefault(q => q.ClassId == @class.Id);

            if (scheduleControl == null)
            {
                foreach (ClassSession classSession in @class.ClassSessions)
                {
                    RoomSchedule roomSchedule = new()
                    {
                        GeneralDate = classSession.Date,
                        Category = EnumScheduleCategory.Class,
                        ClassId = classSession.ClassId,
                        RoomId = @class.RoomId,
                        DayOfWeek = Convert.ToInt16(classSession.Date.DayOfWeek),
                        DayOfMonth = classSession.Date.Month,
                        Year = classSession.Date.Year,
                        StartTime = classSession.StartTime.ToString(),
                        EndTime = classSession.EndTime.ToString()
                    };
                    await _spaceDbContext.RoomSchedules.AddAsync(roomSchedule);
                }
            }
        }
        await _spaceDbContext.SaveChangesAsync();
    }
}
