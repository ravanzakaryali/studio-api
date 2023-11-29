using Microsoft.AspNetCore.Http;
using Space.Domain.Entities;

namespace Space.Application.Handlers;

public class CreateClassSessionExtensionCommand : IRequest
{
    public CreateClassSessionExtensionCommand(double hours, Guid classId, Guid roomId, IEnumerable<CreateClassSessionDto> sessions, DateOnly? startDate)
    {
        Hours = hours;
        ClassId = classId;
        RoomId = roomId;
        Sessions = sessions;
        StartDate = startDate;
    }

    public DateOnly? StartDate { get; set; }
    public double Hours { get; set; }
    public Guid ClassId { get; set; }
    public Guid RoomId { get; set; }
    public IEnumerable<CreateClassSessionDto> Sessions { get; set; } = null!;
}
public class CreateClassSessionExtensionCommandHandler : IRequestHandler<CreateClassSessionExtensionCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateClassSessionExtensionCommandHandler(
        IUnitOfWork unitOfWork, ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateClassSessionExtensionCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.ClassId)
            .Include(c => c.ClassSessions)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.ClassId);
        Room? room = await _spaceDbContext.Rooms.FindAsync(request.RoomId) ??
            throw new NotFoundException(nameof(Room), request.RoomId);



        List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();
        List<ClassSessions> classSessions = new();

        DateOnly startDate = request.StartDate ?? @class.ClassSessions.MaxBy(c => c.Date)!.Date;
        int startDayOfWeek = (int)startDate.DayOfWeek;
        int count = 0;
        double totalHour = request.Hours;


        //Todo: Code Review
        while (totalHour > 0)
        {
            foreach (var session in request.Sessions.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)session.DayOfWeek - (int)startDayOfWeek + 7) % 7;
                int numSelectedDays = request.Sessions.Count();

                int hour = (session.End - session.Start).Hours;
                DateOnly dateTime = startDate.AddDays(count * 7 + daysToAdd);
                Console.WriteLine(dateTime);

                if (holidayDates.Contains(dateTime))
                {
                    continue;
                }

                if (hour != 0)
                {
                    classSessions.Add(new ClassSessions()
                    {
                        Category = session.Category,
                        ClassId = @class.Id,
                        StartTime = session.Start,
                        EndTime = session.End,
                        RoomId = @class.RoomId,
                        TotalHours = hour,
                        Date = dateTime
                    });
                    if (session.Category != ClassSessionCategory.Lab)
                        totalHour -= hour;
                    if (totalHour <= 0) break;
                }
            }
            count++;
        }

        await _spaceDbContext.ClassSessions.AddRangeAsync(classSessions, cancellationToken);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
