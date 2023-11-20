namespace Space.Application.Handlers;

public class GetClassDetailQuery : IRequest<GetClassDetailResponse>
{
    public Guid Id { get; set; }
    public Guid? SessionId { get; set; }
}

internal class GetClassDetaulQueryHandler : IRequestHandler<GetClassDetailQuery, GetClassDetailResponse>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;

    public GetClassDetaulQueryHandler(ISpaceDbContext spaceDbContext, IUnitOfWork unitOfWork)
    {
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetClassDetailResponse> Handle(GetClassDetailQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.Program)
            .Include(c => c.Session)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.Id);

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.ClassId == @class.Id && c.Status != null && c.Status != ClassSessionStatus.Cancelled)
            .Include(c => c.Attendances)
            .ToListAsync();

        List<double> list = new();
        foreach (ClassSession? item in classSessions.Where(c => c.Attendances.Count > 0))
        {
            var total = item.TotalHour;
            var totalAttendance = item.Attendances.Average(c => c.TotalAttendanceHours);
            list.Add((totalAttendance * 100) / total);
        }

        DateTime? startDate = @class.StartDate;
        DateTime? endDate = @class.EndDate;

        if (request.SessionId != null)
        {
            Session session = await _spaceDbContext.Sessions
                .Where(c => c.Id == request.SessionId)
                .Include(c => c.Details)
                .FirstOrDefaultAsync() ??
                    throw new NotFoundException(nameof(Session), request.SessionId);

            List<DateTime> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();

            (DateTime StartDate, DateTime EndDate) responseDate = _unitOfWork.ClassService.CalculateStartAndEndDate(session, @class, holidayDates);
            startDate = responseDate.StartDate;
            endDate = responseDate.EndDate;
        }

        return new GetClassDetailResponse()
        {
            Session = new GetSessionResponseDto()
            {
                Id = @class.Session.Id,
                Name = @class.Session.Name,
            },
            Program = new GetProgramResponseDto()
            {
                Id = @class.Program.Id,
                Name = @class.Program.Name,
            },
            Name = @class.Name,
            AttendanceRate = Math.Round(list.Count > 0 ? list.Average() : 0, 2),
            EndDate = startDate,
            StartDate = endDate
        };
    }
}
