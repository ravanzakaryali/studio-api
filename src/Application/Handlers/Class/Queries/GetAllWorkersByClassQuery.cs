using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record GetAllWorkersByClassQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetWorkersByClassResponseDto>>;

internal class GetAllWorkersByClassQueryHandler : IRequestHandler<GetAllWorkersByClassQuery, IEnumerable<GetWorkersByClassResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetAllWorkersByClassQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetWorkersByClassResponseDto>> Handle(GetAllWorkersByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _unitOfWork.ClassRepository.GetAsync(request.Id, tracking: false, "ClassModulesWorkers.Worker", "ClassModulesWorkers.Role", "ClassSessions.AttendancesWorkers", "Program.Modules.SubModules")
            ?? throw new NotFoundException(nameof(Class), request.Id);

        IEnumerable<ClassSession> classSessions = await _unitOfWork.ClassSessionRepository
            .GetAllAsync(cs => cs.ClassId == @class.Id &&
            cs.Category != ClassSessionCategory.Lab &&
            request.Date >= cs.Date) ??
            throw new NotFoundException(nameof(ClassSession), request.Id);

        List<Module> modules = @class.Program.Modules
            .OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null)
            .Where(m => m.TopModuleId != null || !m.SubModules.Any())
            .ToList();

        int totalHour = classSessions.Sum(c => c.TotalHour);
        Module? currentModule = null;
        if (totalHour > 0)
        {
            double totalHourModule = 0;

            for (int i = 0; i < modules.Count; i++)
            {
                totalHourModule += modules[i].Hours;
                if (totalHourModule >= totalHour)
                {
                    currentModule = modules[i];
                    break;
                }
            }
            currentModule ??= modules.LastOrDefault(); ;
        }
        else
        {
            currentModule = modules.FirstOrDefault();
        }
        return @class.ClassModulesWorkers.Where(c => c.ModuleId == currentModule.Id).Distinct(new GetModulesWorkerComparer()).Select(c =>
        {
            ClassSession? classSession = @class.ClassSessions.FirstOrDefault(c => c.Date == request.Date);
            bool isAttendance = false;
            if (classSession != null)
            {
                var attendance = classSession.AttendancesWorkers.FirstOrDefault(attendance => attendance.WorkerId == c.WorkerId);
                if (attendance != null)
                {
                    isAttendance = attendance.TotalAttendanceHours == classSession.TotalHour;
                }
            }
            return new GetWorkersByClassResponseDto()
            {
                Name = c.Worker.Name!,
                Surname = c.Worker.Surname!,
                RoleId = c.RoleId,
                RoleName = c.Role!.Name,
                WorkerId = c.WorkerId,
                IsAttendance = isAttendance,
                TotalLessonHours = @class.ClassSessions.Where(session => session.Status == ClassSessionStatus.Offline || session.Status == ClassSessionStatus.Online).SelectMany(c => c.AttendancesWorkers).Where(attendance => attendance.WorkerId == c.WorkerId).Sum(c => c.TotalAttendanceHours)
            };
        });
    }
}
public class GetModulesWorkerComparer : IEqualityComparer<ClassModulesWorker>
{
    public bool Equals(ClassModulesWorker x, ClassModulesWorker y)
    {
        return x.WorkerId == y.WorkerId && x.RoleId == y.RoleId;
    }

    public int GetHashCode(ClassModulesWorker obj)
    {
        return obj.WorkerId.GetHashCode() ^ obj.RoleId.GetHashCode();
    }
}