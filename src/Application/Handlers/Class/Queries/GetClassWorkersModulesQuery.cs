using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace Space.Application.Handlers;

public record GetClassWorkersModulesQuery(Guid Id, Guid SessionId) : IRequest<IEnumerable<GetClassModuleResponseDto>>;

internal class GetClassWorkersModulesQueryHandler : IRequestHandler<GetClassWorkersModulesQuery, IEnumerable<GetClassModuleResponseDto>>
{
    readonly IMapper _mapper;
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;
    readonly ICurrentUserService _currentUserService;

    public GetClassWorkersModulesQueryHandler(
        IMapper mapper,
        ICurrentUserService currentUserService,
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetClassModuleResponseDto>> Handle(GetClassWorkersModulesQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.Program)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.Id);

        Session? session = await _spaceDbContext.Sessions
            .Where(c => c.Id == request.SessionId)
            .Include(c => c.Details)
            .FirstOrDefaultAsync() ??
            throw new NotFoundException(nameof(Session), request.SessionId);

        List<Module> modules = await _spaceDbContext.Modules
            .Where(c => c.ProgramId == @class.ProgramId && c.TopModuleId == null)
            .AsNoTracking()
            .ToListAsync();
        List<ClassModulesWorker> classModulesWorkers = await _spaceDbContext.ClassModulesWorkers
            .Where(c => c.ClassId == @class.Id)
            .Include(c => c.Role)
            .Include(c => c.Worker)
            .ToListAsync();

        List<GetClassModuleResponseDto> response = modules.Select(m => new GetClassModuleResponseDto()
        {
            Id = m.Id,
            Name = m.Name,
            Hours = m.Hours,
            Version = m.Version,
            SubModules = m.SubModules?.Select(sub => new SubModuleDtoWithWorker()
            {
                Name = sub.Name,
                Hours = sub.Hours,
                Id = sub.Id,
                TopModuleId = sub.TopModuleId,
                Version = sub.Version,
                Workers = _mapper.Map<ICollection<GetWorkerForClassDto>>(classModulesWorkers.Where(cmw => cmw.ModuleId == sub.Id))
            }).ToList(),
            Workers = _mapper.Map<ICollection<GetWorkerForClassDto>>(classModulesWorkers.Where(cmw => m.SubModules!.Any(s => s.Id == cmw.ModuleId)).Distinct(new GetWorkerForClassDtoComparer()))
        }).ToList();

        List<DateHourDto> classDateTimes = new();
        DateTime startDateTime = @class.StartDate ?? DateTime.Now;

        int count = 0;
        List<DateTime> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();
        int totalHour = @class.Program.TotalHours;

        while (totalHour > 0)
        {
            foreach (var sessionItem in session.Details.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)sessionItem.DayOfWeek - (int)startDateTime.DayOfWeek + 7) % 7;
                int numSelectedDays = session.Details.Count;

                int hour = (sessionItem.EndTime - sessionItem.StartTime).Hours;


                DateTime dateTime = startDateTime.AddDays(count * 7 + daysToAdd);

                if (holidayDates.Contains(dateTime))
                {
                    continue;
                }

                if (hour != 0)
                {
                    classDateTimes.Add(new DateHourDto()
                    {
                        DateTime = dateTime,
                        Hour = hour,
                    });
                    totalHour -= hour;
                    if (totalHour <= 0)
                        break;

                }
            }
            count++;
        }

        classDateTimes = classDateTimes.OrderBy(c => c.DateTime).ToList();

        response = response.OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null).OrderBy(c => c.Version).ToList();
        foreach (var item in response)
        {
            item.SubModules = item.SubModules?.OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null).ToList();
        }
        //Todo: Code review
        response.First().StartDate = @class.StartDate ?? DateTime.Now;
        response.Last().EndDate = classDateTimes.Last().DateTime;

        for (int i = 0; i < response.Count; i++)
        {
            double totalModuleHour = response[i].Hours;
            int sum = 0;
            foreach (var item in classDateTimes.Where(c => i > 0 ? c.DateTime > response[i - 1].EndDate : true))
            {
                sum += item.Hour;
                if (i > 0) response[i].StartDate = response[i - 1].EndDate;
                if (sum >= response[i].Hours)
                {
                    response[i].EndDate = item.DateTime;
                    sum = 0;
                    break;
                }
            }
            if (response[i].SubModules != null)
            {
                for (int j = 0; j < response[i].SubModules!.Count; j++)
                {
                    int subModuleSum = 0;
                    double totalSubModuleHour = response[i].SubModules![j].Hours;
                    response[i].SubModules![0].StartDate = response[i].StartDate;
                    if (j == response[i].SubModules!.Count - 1)
                    {
                        response[i].SubModules![j].EndDate = response[i].EndDate;
                    }
                    foreach (var item in classDateTimes.Where(c => j > 0 ? c.DateTime > response[i].SubModules![j - 1].EndDate : true))
                    {
                        subModuleSum += item.Hour;
                        if (subModuleSum >= response[i].SubModules![j].Hours)
                        {
                            response[i].SubModules![j].EndDate = item.DateTime;
                            if (j > 0) response[i].SubModules![j].StartDate = response[i].SubModules![j - 1].EndDate;
                            subModuleSum = 0;
                            break;
                        }
                    }
                }
            }
        }


        return response;
    }
}

public class GetWorkerForClassDtoComparer : IEqualityComparer<ClassModulesWorker>
{
    public bool Equals(ClassModulesWorker? x, ClassModulesWorker? y)
    {
        return x?.WorkerId == y?.WorkerId && x?.RoleId == y?.RoleId;
    }

    public int GetHashCode(ClassModulesWorker obj)
    {
        return obj.WorkerId.GetHashCode() ^ obj.RoleId.GetHashCode();
    }
}
public class DateHourDto
{
    public DateTime DateTime { get; set; }
    public int Hour { get; set; }
}