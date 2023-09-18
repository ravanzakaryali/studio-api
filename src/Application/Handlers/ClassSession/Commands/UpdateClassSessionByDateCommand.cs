using System.Runtime.ExceptionServices;

namespace Space.Application.Handlers;

public record UpdateClassSessionByDateCommand(Guid ClassId, DateOnly OldDate, DateOnly NewDate, IEnumerable<UpdateClassSessionDto> Sessions) : IRequest;

internal class UpdateClassSessionByDateCommandHandler : IRequestHandler<UpdateClassSessionByDateCommand>
{
    readonly IUnitOfWork _unitOfWork;

    public UpdateClassSessionByDateCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateClassSessionByDateCommand request, CancellationToken cancellationToken)
    {
        DateTime newDateTime = request.NewDate.ToDateTime(new TimeOnly(0, 0));
        DateTime oldDateTime = request.OldDate.ToDateTime(new TimeOnly(0, 0));

        if (request.Sessions.Count() > 2) throw new Exception("There cannot be more than two sessions in one day");

        IEnumerable<ClassSession> classSessions = await _unitOfWork.ClassSessionRepository.GetAllAsync(c => c.ClassId == request.ClassId && c.Date == oldDateTime && (c.Status == null || c.Status == ClassSessionStatus.Cancelled));

        classSessions.ToList().ForEach(c => c.Status = null);

        if (!classSessions.Any()) throw new Exception("Either a session with the old date is entered or there is no such session");

        int totalHour = classSessions.Sum(c => c.TotalHour);
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
                    classSession.TotalHour = (requestSession.EndTime - requestSession.StartTime).Hours;
                    ClassSession? difCalsssSession = classSessions.FirstOrDefault(c => c.Category != requestSession.Category);
                    if ((requestSession.EndTime - requestSession.StartTime).Hours == totalHour && difCalsssSession != null)
                    {
                        _unitOfWork.ClassSessionRepository.Remove(difCalsssSession, true);
                    }
                }
                else
                {
                    ClassSession? difCalsssSession = classSessions.FirstOrDefault(c => c.Category != requestSession.Category);
                    if ((requestSession.EndTime - requestSession.StartTime).Hours == totalHour && difCalsssSession != null)
                    {
                        _unitOfWork.ClassSessionRepository.Remove(difCalsssSession, true);
                    }
                    await _unitOfWork.ClassSessionRepository.AddAsync(new ClassSession()
                    {
                        ClassId = firstClassSession.ClassId,
                        Category = requestSession.Category,
                        RoomId = firstClassSession.RoomId,
                        ModuleId = firstClassSession.ModuleId,
                        Date = firstClassSession.Date,
                        Note = firstClassSession.Note,
                        EndTime = requestSession.EndTime,
                        StartTime = requestSession.StartTime,
                        TotalHour = (requestSession.EndTime - requestSession.StartTime).Hours,
                        //WorkerId = firstClassSession.WorkerId,
                    });
                }
            }
        }
        else
        {
            IEnumerable<ClassSession> alreadyClassSession = await _unitOfWork.ClassSessionRepository.GetAllAsync(c => c.ClassId == request.ClassId && c.Date == newDateTime);
            if (alreadyClassSession.Any()) throw new Exception("There is already a session on the specified day");

            IEnumerable<Holiday> alreadyHoliday = await _unitOfWork.HolidayRepository.GetAllAsync(h => h.StartDate >= request.NewDate && h.EndDate <= request.NewDate);
            if (alreadyHoliday.Any()) throw new Exception("There is already a session on the holiday");

            foreach (UpdateClassSessionDto requestSession in request.Sessions)
            {
                ClassSession firstClassSession = classSessions.First();
                ClassSession? classSession = classSessions.FirstOrDefault(c => c.Category == requestSession.Category);
                ClassSession? lastClassSession = classSessions.OrderByDescending(c => c.Date).FirstOrDefault();

                if (classSession != null)
                {
                    classSession.Date = newDateTime;

                    classSession.StartTime = requestSession.StartTime;
                    classSession.EndTime = requestSession.EndTime;
                    classSession.TotalHour = (requestSession.EndTime - requestSession.StartTime).Hours;
                    ClassSession? difCalsssSession = classSessions.FirstOrDefault(c => c.Category != requestSession.Category);
                    if ((requestSession.EndTime - requestSession.StartTime).Hours == totalHour && difCalsssSession != null)
                    {
                        _unitOfWork.ClassSessionRepository.Remove(difCalsssSession, true);
                    }
                }
                else
                {
                    ClassSession? difCalsssSession = classSessions.FirstOrDefault(c => c.Category != requestSession.Category);
                    if ((requestSession.EndTime - requestSession.StartTime).Hours == totalHour && difCalsssSession != null)
                    {
                        _unitOfWork.ClassSessionRepository.Remove(difCalsssSession, true);
                    }
                    await _unitOfWork.ClassSessionRepository.AddAsync(new ClassSession()
                    {
                        ClassId = firstClassSession.ClassId,
                        Category = requestSession.Category,
                        RoomId = firstClassSession.RoomId,
                        ModuleId = firstClassSession.ModuleId,
                        LastModifiedBy = firstClassSession.LastModifiedBy,
                        LastModifiedDate = firstClassSession.LastModifiedDate,
                        CreatedDate = firstClassSession.CreatedDate,
                        CreatedBy = firstClassSession.CreatedBy,
                        Date = newDateTime,
                        Note = firstClassSession.Note,
                        EndTime = requestSession.EndTime,
                        StartTime = requestSession.StartTime,
                        TotalHour = (requestSession.EndTime - requestSession.StartTime).Hours,
                        //WorkerId = firstClassSession.WorkerId,
                    });
                }

            }
        }

        Class @class = await _unitOfWork.ClassRepository.GetAsync(r => r.Id == request.ClassId && r.ClassSessions.Count != 0, true, "ClassSessions")
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        @class.EndDate = @class.ClassSessions.Max(c => c.Date).Date;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
