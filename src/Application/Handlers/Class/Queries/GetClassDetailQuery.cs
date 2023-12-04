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

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == @class.Id && c.Status != ClassSessionStatus.Cancelled)
            .Include(c => c.Attendances)
            .ToListAsync();

        List<double> list = new();
        foreach (ClassTimeSheet? item in classTimeSheets.Where(c => c.Attendances.Count > 0))
        {
            var total = item.TotalHours;
            var totalAttendance = item.Attendances.Average(c => c.TotalAttendanceHours);
            list.Add((totalAttendance * 100) / total);
        }

        DateOnly startDate = @class.StartDate;
        DateOnly? endDate = @class.EndDate;

        if (request.SessionId != null)
        {
            Session session = await _spaceDbContext.Sessions
                .Where(c => c.Id == request.SessionId)
                .Include(c => c.Details)
                .FirstOrDefaultAsync() ??
                    throw new NotFoundException(nameof(Session), request.SessionId);

            List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();

            (DateOnly StartDate, DateOnly EndDate) responseDate = _unitOfWork.ClassService.CalculateStartAndEndDate(session, @class, holidayDates);
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
            TotalHours = @class.Program.TotalHours,
            Name = @class.Name,
            AttendanceRate = Math.Round(list.Count > 0 ? list.Average() : 0, 2),
            EndDate = endDate,
            StartDate = startDate
        };
    }
}
