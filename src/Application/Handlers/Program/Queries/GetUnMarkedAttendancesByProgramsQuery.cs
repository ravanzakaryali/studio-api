using Space.Application.Enums;

namespace Space.Application.Handlers;

public class GetUnMarkedAttendancesByProgramsQuery : IRequest<IEnumerable<GetUnMarkedAttendancesByProgramsDto>>
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

internal class GetUnMarkedAttendancesByProgramsHandler : IRequestHandler<GetUnMarkedAttendancesByProgramsQuery, IEnumerable<GetUnMarkedAttendancesByProgramsDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;

    public GetUnMarkedAttendancesByProgramsHandler(ISpaceDbContext spaceDbContext, IUnitOfWork unitOfWork)
    {
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetUnMarkedAttendancesByProgramsDto>> Handle(GetUnMarkedAttendancesByProgramsQuery request, CancellationToken cancellationToken)
    {
        List<Program> programs = await _spaceDbContext.Programs.ToListAsync(cancellationToken: cancellationToken);
        DateOnly startDate = DateOnly.FromDateTime(request.StartDate);
        DateOnly endDate = DateOnly.FromDateTime(request.EndDate);

        List<Class> classes = await _spaceDbContext.Classes
            .Include(c => c.Session)
            .ThenInclude(s => s.Details)
            .ToListAsync(cancellationToken);

        List<DateOnly> holidays = await _unitOfWork.HolidayService.GetDatesAsync();

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(cts => cts.Date >= startDate && cts.Date <= endDate)
            .ToListAsync(cancellationToken);

        return programs.Select(program =>
        {
            List<Class> programClasses = classes.Where(c => c.ProgramId == program.Id).ToList();
            int unmarkedCount = 0;
            int totalSessions = 0;
            List<Class> totalClasses = new();

            foreach (Class? classEntity in programClasses)
            {
                List<DateOnly> relevantDates = new();

                DateOnly startDateClass = classEntity.StartDate > startDate ? classEntity.StartDate : startDate;
                DateOnly? endDateClass = classEntity.EndDate is not null && classEntity.EndDate < endDate ? classEntity.EndDate : endDate;

                for (DateOnly date = startDateClass; date <= endDateClass; date = date.AddDays(1))
                {
                    DayOfWeek dayOfWeek = date.DayOfWeek;
                    bool hasSession = classEntity.Session.Details.Any(sd => sd.DayOfWeek == dayOfWeek);
                    if (hasSession && !holidays.Contains(date))
                    {
                        relevantDates.Add(date);
                    }
                }
                totalSessions += relevantDates.Count;

                foreach (DateOnly date in relevantDates.Where(date => date < DateOnly.FromDateTime(DateTime.Now)))
                {
                    ClassTimeSheet? classTimeSheet = classTimeSheets
                        .FirstOrDefault(cts => cts.ClassId == classEntity.Id && cts.Date == date);

                    if (classTimeSheet == null)
                    {
                        totalClasses.Add(classEntity);
                        unmarkedCount++;
                    }
                }
            }

            double totalAttendancePercentage = totalSessions > 0
                ? (double)(totalSessions - unmarkedCount) / totalSessions * 100
                : 0;

            return new GetUnMarkedAttendancesByProgramsDto
            {
                Program = new GetProgramResponseDto
                {
                    Id = program.Id,
                    Name = program.Name
                },
                UnMarkedAttendancesCount = unmarkedCount,
                TotalUnMarkedAttendancesCount = totalClasses.DistinctBy(c => c.Id).Count(),
                TotalAttendancePercentage = Math.Round(totalAttendancePercentage, MidpointRounding.AwayFromZero)
            };
        })
        .OrderByDescending(dto => dto.TotalUnMarkedAttendancesCount)
        .ThenByDescending(dto => dto.TotalAttendancePercentage)
        .ToList();
    }
}
