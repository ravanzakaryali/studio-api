using System.Runtime.ExceptionServices;

namespace Space.Application.Handlers;

public record UpdateClassSessionOneDayCommand(int ClassId, DateOnly OldDate, DateOnly NewDate, IEnumerable<UpdateClassSessionDto> Sessions) : IRequest;

internal class UpdateClassSessionOneDayCommandHandlder : IRequestHandler<UpdateClassSessionOneDayCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;

    public UpdateClassSessionOneDayCommandHandlder(IUnitOfWork unitOfWork, ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(UpdateClassSessionOneDayCommand request, CancellationToken cancellationToken)
    {

        if (request.Sessions.Count() > 2) throw new Exception("There cannot be more than two sessions in one day");

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
                                .Where(c => c.ClassId == request.ClassId && c.Date == request.OldDate && (c.ClassTimeSheet == null || c.Status == ClassSessionStatus.Cancelled))
                                .ToListAsync(cancellationToken: cancellationToken);

        if (!classSessions.Any()) throw new Exception("Either a session with the old date is entered or there is no such session");

        int totalHour = classSessions.Sum(c => c.TotalHours);
        if (totalHour != request.Sessions.Sum(c => (c.EndTime - c.StartTime).Hours)) throw new Exception("The total hours must be equal to the total hours of the sessions");

        if (request.OldDate == request.NewDate)
        {
            foreach (UpdateClassSessionDto requestSession in request.Sessions)
            {
                ClassSession firstClassSession = classSessions.First();
                ClassSession? classSession = classSessions.FirstOrDefault(c => c.Category == requestSession.Category);

                if (classSession != null)
                {
                    classSession.StartTime = requestSession.StartTime;
                    classSession.EndTime = requestSession.EndTime;
                    classSession.TotalHours = (requestSession.EndTime - requestSession.StartTime).Hours;
                    ClassSession? difCalsssSession = classSessions.FirstOrDefault(c => c.Category != requestSession.Category);
                    if ((requestSession.EndTime - requestSession.StartTime).Hours == totalHour && difCalsssSession != null)
                    {
                        _spaceDbContext.ClassSessions.Remove(difCalsssSession);
                    }
                }
                else
                {
                    ClassSession? difCalsssSession = classSessions.FirstOrDefault(c => c.Category != requestSession.Category);
                    if ((requestSession.EndTime - requestSession.StartTime).Hours == totalHour && difCalsssSession != null)
                    {
                        _spaceDbContext.ClassSessions.Remove(difCalsssSession);
                    }
                    await _spaceDbContext.ClassSessions.AddAsync(new ClassSession()
                    {
                        ClassId = firstClassSession.ClassId,
                        Category = requestSession.Category,
                        RoomId = firstClassSession.RoomId,
                        Date = firstClassSession.Date,
                        Status = ClassSessionStatus.Offline,
                        ClassTimeSheetId = null,
                        EndTime = requestSession.EndTime,
                        ClassTimeSheet = null,
                        StartTime = requestSession.StartTime,
                        TotalHours = (requestSession.EndTime - requestSession.StartTime).Hours,
                    });
                }
            }
        }
        else
        {
            IEnumerable<ClassSession> alreadyClassSession = await _spaceDbContext.ClassSessions
                            .Where(c => c.ClassId == request.ClassId && c.Date == request.NewDate)
                            .ToListAsync(cancellationToken: cancellationToken);
            if (alreadyClassSession.Any()) throw new Exception("There is already a session on the specified day");

            IEnumerable<Holiday> alreadyHoliday = await _spaceDbContext.Holidays.Where(h => h.StartDate >= request.NewDate && h.EndDate <= request.NewDate).ToListAsync();
            if (alreadyHoliday.Any()) throw new Exception("There is already a session on the holiday");

            foreach (UpdateClassSessionDto requestSession in request.Sessions)
            {
                ClassSession firstClassSession = classSessions.First();
                ClassSession? classSession = classSessions.FirstOrDefault(c => c.Category == requestSession.Category);
                ClassSession? lastClassSession = classSessions.OrderByDescending(c => c.Date).FirstOrDefault();

                if (classSession != null)
                {
                    classSession.Date = request.NewDate;

                    classSession.StartTime = requestSession.StartTime;
                    classSession.EndTime = requestSession.EndTime;
                    classSession.TotalHours = (requestSession.EndTime - requestSession.StartTime).Hours;
                    ClassSession? difCalsssSession = classSessions.FirstOrDefault(c => c.Category != requestSession.Category);
                    if ((requestSession.EndTime - requestSession.StartTime).Hours == totalHour && difCalsssSession != null)
                    {
                        _spaceDbContext.ClassSessions.Remove(difCalsssSession);
                    }
                }
                else
                {
                    ClassSession? difCalsssSession = classSessions.FirstOrDefault(c => c.Category != requestSession.Category);
                    if ((requestSession.EndTime - requestSession.StartTime).Hours == totalHour && difCalsssSession != null)
                    {
                        _spaceDbContext.ClassSessions.Remove(difCalsssSession);
                    }

                    await _spaceDbContext.ClassSessions.AddAsync(new ClassSession()
                    {
                        ClassId = firstClassSession.ClassId,
                        Category = requestSession.Category,
                        RoomId = firstClassSession.RoomId,
                        LastModifiedBy = firstClassSession.LastModifiedBy,
                        LastModifiedDate = firstClassSession.LastModifiedDate,
                        CreatedDate = firstClassSession.CreatedDate,
                        CreatedBy = firstClassSession.CreatedBy,
                        Date = request.NewDate,
                        ClassTimeSheet = null,
                        ClassTimeSheetId = null,
                        Status = ClassSessionStatus.Offline,
                        EndTime = requestSession.EndTime,
                        StartTime = requestSession.StartTime,
                        TotalHours = (requestSession.EndTime - requestSession.StartTime).Hours,
                    });
                }

            }
        }

        Class @class = await _spaceDbContext.Classes
                        .Where(r => r.Id == request.ClassId && r.ClassSessions.Count != 0)
                        .Include(r => r.ClassSessions)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        @class.EndDate = @class.ClassSessions.Max(c => c.Date);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
