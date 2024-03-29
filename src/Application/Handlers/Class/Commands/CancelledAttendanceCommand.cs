using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Space.Application.Handlers.Commands;

public record CancelledAttendanceCommand(
        int ClassId, DateTime Date) : IRequest;
internal class CancelledAttendanceHandler : IRequestHandler<CancelledAttendanceCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;

    public CancelledAttendanceHandler(
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork)
    {
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelledAttendanceCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
                        .Include(c => c.Session)
                        .ThenInclude(c => c.Details)
                        .FirstOrDefaultAsync(c => c.Id == request.ClassId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        DateOnly date = DateOnly.FromDateTime(request.Date);

        //əgər class session ləğv olunarsa buradan ləğv olunur 
        //əgər yenidən həmin gün dərs olarsa yenidən yaradılır
        //amma sonda bir gün əlavə etmək məsələsinə gəldikdə isə
        //cancelledDate deyə bir session sağlayacam
        //davamiyyət daxil olunanda əgər ki cancelled date class session içərisində varsa onu silsin.
        //əgər ki yoxdursa onda davamiyyətə əlavə etsin

        IEnumerable<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.ClassId == request.ClassId && c.Date == date)
            .ToListAsync(cancellationToken: cancellationToken);

        if (!classSessions.Any())
        {
            throw new NotFoundException(nameof(ClassSession), request.ClassId);
        }
        IEnumerable<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == request.ClassId && c.Date == date)
            .ToListAsync(cancellationToken: cancellationToken);

        List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();
        DateOnly classLastDate = await _unitOfWork.ClassSessionService.GetLastDateAsync(@class.Id);




        foreach (ClassSession classSession in classSessions)
        {
            classSession.Status = ClassSessionStatus.Cancelled;
            DateOnly date2 = classLastDate.AddDays(1);
            DayOfWeek startDateDayOfWeek = date2.DayOfWeek;
            while (
                      !@class.Session.Details.Select(c => c.DayOfWeek).Any(c => date2.DayOfWeek == c)
                  )
            {
                date2 = date2.AddDays(1);
                startDateDayOfWeek = date2.DayOfWeek;
            }
            List<ClassSession> generateClassSessions = _unitOfWork.ClassSessionService
                            .GenerateSessions(
                                classSession.TotalHours,
                                classSessions
                                    .Select(
                                        r =>
                                            new CreateClassSessionDto()
                                            {
                                                Category = r.Category,
                                                DayOfWeek = startDateDayOfWeek,
                                                End = classSession.EndTime,
                                                Start = classSession.StartTime
                                            }
                                    )
                                    .ToList(),
                                date2,
                                holidayDates,
                                @class.Id,
                                classSession.RoomId!.Value
                            );
            await _spaceDbContext.ClassSessions.AddRangeAsync(generateClassSessions, cancellationToken);
            if (generateClassSessions.Any())
                @class.EndDate = generateClassSessions.OrderByDescending(c => c.Date).FirstOrDefault()?.Date;
        }



        _spaceDbContext.ClassTimeSheets.RemoveRange(classTimeSheets);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
