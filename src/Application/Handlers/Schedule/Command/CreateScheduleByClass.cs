using System;

namespace Space.Application.Handlers;

public record CreateRoomScheduleByClassCommand : IRequest;



internal class CreateRoomScheduleByClassCommandHandler : IRequestHandler<CreateRoomScheduleByClassCommand>
{
    readonly IUnitOfWork _unitOfWork;

    public CreateRoomScheduleByClassCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateRoomScheduleByClassCommand request, CancellationToken cancellationToken)
    {

        IEnumerable<Class> allClasses = await _unitOfWork.ClassRepository.GetAllAsync(predicate: null, tracking: false, "ClassSessions", "RoomSchedules");

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
                    await _unitOfWork.RoomScheduleRepository.AddAsync(roomSchedule);
                }
            }
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
