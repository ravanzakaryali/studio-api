using System;

namespace Space.Application.Handlers;

public record CreateRoomScheduleByClassCommand : IRequest;



internal class CreateRoomScheduleByClassCommandHandler : IRequestHandler<CreateRoomScheduleByClassCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IClassRepository _classRepository;
    readonly IRoomScheduleRepository _roomScheduleRepository;

    public CreateRoomScheduleByClassCommandHandler(
                                IUnitOfWork unitOfWork,
                                IClassRepository classRepository,
                                IRoomScheduleRepository roomScheduleRepository)
    {
        _unitOfWork = unitOfWork;
        _classRepository = classRepository;
        _roomScheduleRepository = roomScheduleRepository;
    }

    public async Task Handle(CreateRoomScheduleByClassCommand request, CancellationToken cancellationToken)
    {

        IEnumerable<Class> allClasses = await _classRepository.GetAllAsync(predicate: null, tracking: false, "ClassSessions", "RoomSchedules");

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
                    await _roomScheduleRepository.AddAsync(roomSchedule);
                }
            }
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
