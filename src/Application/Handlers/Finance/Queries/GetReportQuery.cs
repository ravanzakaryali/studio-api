
namespace Space.Application.Handlers;

public class GetReportQuery : IRequest<IEnumerable<GetFincanceReportGetDto>>
{
    public MonthOfYear? Month { get; set; }
    public string? Role { get; set; }
}

internal class GetReportQueryHandler : IRequestHandler<GetReportQuery, IEnumerable<GetFincanceReportGetDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetReportQueryHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetFincanceReportGetDto>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        // Ay bilgisi null ise, mevcut ayı kullan
        int year = DateTime.Now.Year;
        int month = request.Month.HasValue ? (int)request.Month.Value : DateTime.Now.Month;

        DateOnly startDate = new DateOnly(year, month, 1);
        int lastDay = DateTime.DaysInMonth(year, month);
        DateOnly endDate = new DateOnly(year, month, lastDay);

        List<ClassModulesWorker> classModulesWorkers = await _spaceDbContext.ClassModulesWorkers
            .Include(c => c.Role)
            .ToListAsync();

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Include(x => x.Class)
            .Include(x => x.Class.Program)
            .Include(x => x.AttendancesWorkers)
            .ThenInclude(aw => aw.Worker)
            .Where(x => x.Date >= startDate && x.Date <= endDate)
            .ToListAsync();

        List<GetFincanceReportGetDto> groupedData = classTimeSheets
            .SelectMany(cts => cts.AttendancesWorkers, (cts, aw) => new
            {
                aw.Worker.Name,
                aw.Worker.Surname,
                aw.Worker.Fincode,
                Id = aw.Worker.Id,
                ProgramName = cts.Class.Program.Name,
                GroupName = cts.Class.Name,
                aw.TotalHours,
                aw.TotalMinutes
            })
            .GroupBy(x => new { x.Name, x.Surname, x.Fincode, x.ProgramName, x.GroupName, x.Id })
            .Select(g =>
            {
                int totalMinutes = g.Sum(x => x.TotalMinutes);
                int totalHours = g.Sum(x => x.TotalHours) + totalMinutes / 60;
                totalMinutes = totalMinutes % 60;

                return new GetFincanceReportGetDto
                {
                    Id = g.Key.Id,
                    Name = g.Key.Name,
                    Surname = g.Key.Surname,
                    Fincode = g.Key.Fincode,
                    ProgramName = g.Key.ProgramName,
                    GroupName = g.Key.GroupName,
                    TotalHours = totalHours,
                    TotalMinutes = totalMinutes,
                };
            })
            .ToList();

        foreach (GetFincanceReportGetDto? item in groupedData)
        {
            item.Role = classModulesWorkers.FirstOrDefault(x => x.WorkerId == item.Id)?.Role?.Name;
        }

        if (request.Role != null)
        {
            groupedData = groupedData.Where(x => x.Role == request.Role).ToList();
        }

        return groupedData.OrderByDescending(x => x.TotalHours).ThenByDescending(x => x.TotalMinutes);
    }
}