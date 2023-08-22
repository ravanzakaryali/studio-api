using Microsoft.AspNetCore.Http;
using Space.Domain.Entities;

namespace Space.Application.Handlers;

public class CreateClassSessionExtensionCommand : IRequest
{
    public CreateClassSessionExtensionCommand(double hours, Guid classId, Guid roomId, IEnumerable<CreateClassSessionDto> sessions)
    {
        Hours = hours;
        ClassId = classId;
        RoomId = roomId;
        Sessions = sessions;
    }

    public double Hours { get; set; }
    public Guid ClassId { get; set; }
    public Guid RoomId { get; set; }
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}
public class CreateClassSessionExtensionCommandHandler : IRequestHandler<CreateClassSessionExtensionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateClassSessionExtensionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateClassSessionExtensionCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _unitOfWork.ClassRepository.GetAsync(c => c.Id == request.ClassId, true, "ClassSessions") ??
            throw new NotFoundException(nameof(Class), request.ClassId);
        Room room = await _unitOfWork.RoomRepository.GetAsync(r => r.Id == request.RoomId) ??
            throw new NotFoundException(nameof(Room), request.RoomId);

        IEnumerable<Holiday> holidays = await _unitOfWork.HolidayRepository.GetAllAsync();
        List<DateTime> holidayDates = new();
        foreach (Holiday holiday in holidays)
        {
            for (DateOnly date = holiday.StartDate; date <= holiday.EndDate; date = date.AddDays(1))
            {
                holidayDates.Add(date.ToDateTime(new TimeOnly(0, 0)));
            }
        }
        List<ClassSession> classSessions = new();

        DateTime startDate = @class.ClassSessions.MaxBy(c => c.Date)!.Date;
        int startDayOfWeek = (int)startDate.DayOfWeek;
        int count = 0;
        double totalHour = request.Hours;

        while (totalHour > 0)
        {
            foreach (var session in request.Sessions.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)session.DayOfWeek - (int)startDayOfWeek + 7) % 7;
                int numSelectedDays = request.Sessions.Count();

                int hour = (session.End - session.Start).Hours;
                DateTime dateTime = startDate.AddDays(count * 7 + daysToAdd);
                Console.WriteLine(dateTime);

                if (holidayDates.Contains(dateTime))
                {
                    continue;
                }

                if (hour != 0)
                {
                    classSessions.Add(new ClassSession()
                    {
                        Category = session.Category,
                        ClassId = @class.Id,
                        StartTime = session.Start,
                        EndTime = session.End,
                        RoomId = @class.Room.Id,
                        TotalHour = hour,
                        Date = dateTime
                    });
                    if (session.Category != ClassSessionCategory.Lab)
                        totalHour -= hour;
                    if (totalHour <= 0) break;
                }
            }
            count++;
        }

        await _unitOfWork.ClassSessionRepository.AddRangeAsync(classSessions);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
