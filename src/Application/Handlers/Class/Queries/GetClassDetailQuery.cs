namespace Space.Application.Handlers;

public class GetClassDetailQuery : IRequest<GetClassDetailResponse>
{
    public Guid Id { get; set; }
    public Guid? SessionId { get; set; }
}

internal class GetClassDetaulQueryHandler : IRequestHandler<GetClassDetailQuery, GetClassDetailResponse>
{
    readonly IClassRepository _classRepository;
    readonly IClassSessionRepository _classSessionRepository;
    readonly ISessionRepository _sessionRepository;
    readonly IHolidayRepository _holidayRepository;

    public GetClassDetaulQueryHandler(IClassRepository classRepository,
                                      IClassSessionRepository classSessionRepository,
                                      ISessionRepository sessionRepository,
                                      IHolidayRepository holidayRepository)
    {
        _classRepository = classRepository;
        _classSessionRepository = classSessionRepository;
        _sessionRepository = sessionRepository;
        _holidayRepository = holidayRepository;
    }

    public async Task<GetClassDetailResponse> Handle(GetClassDetailQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _classRepository.GetAsync(r => r.Id == request.Id, false, "Program", "Session")
                            ?? throw new NotFoundException(nameof(Class), request.Id);

        IEnumerable<ClassSession> classSessoins = await _classSessionRepository.GetAllAsync(c =>
                                                                                                c.ClassId == @class.Id &&
                                                                                                c.Status != null &&
                                                                                                c.Status != ClassSessionStatus.Cancelled,
                                                                                                false,
                                                                                                "Attendances");
        List<double> list = new();
        foreach (ClassSession? item in classSessoins.Where(c => c.Attendances.Count > 0))
        {
            var total = item.TotalHour;
            var totalAttendance = item.Attendances.Average(c => c.TotalAttendanceHours);
            list.Add((totalAttendance * 100) / total);
        }

        DateTime? startDate = @class.StartDate;
        DateTime? endDate = @class.EndDate;

        if (request.SessionId != null)
        {
            Session session = await _sessionRepository.GetAsync(s => s.Id == request.SessionId, false, "Details")
                ?? throw new NotFoundException(nameof(Session), request.SessionId);

            List<DateTime> holidayDates = await _holidayRepository.GetDatesAsync();

            (DateTime StartDate, DateTime EndDate) responseDate = _classRepository.CalculateStartAndEndDate(session, @class, holidayDates);
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
