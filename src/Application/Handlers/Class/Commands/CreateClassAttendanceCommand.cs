using Space.Application.Abstractions;
using Space.Domain.Entities;

namespace Space.Application.Handlers;

public class CreateClassAttendanceCommand : IRequest
{
    public Guid ClassId { get; set; }
    public Guid ModuleId { get; set; }
    public DateTime Date { get; set; }
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; }
}
internal class CreateClassAttendanceCommandHandler : IRequestHandler<CreateClassAttendanceCommand>
{
    readonly IUnitOfWork _unitOfWork;

    public CreateClassAttendanceCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateClassAttendanceCommand request, CancellationToken cancellationToken)
    {

        Class @class = await _unitOfWork.ClassRepository.GetAsync(request.ClassId, tracking: false, "Studies", "Program.Modules.SubModules", "ClassModulesWorkers.Worker", "ClassModulesWorkers.Role") ??
            throw new NotFoundException(nameof(Class), request.ClassId);

        Module module = await _unitOfWork.ModuleRepository.GetAsync(request.ModuleId, tracking: false) ??
            throw new NotFoundException(nameof(Module), request.ModuleId);

        IEnumerable<ClassSession> classSessionsHour = await _unitOfWork.ClassSessionRepository.GetAllAsync(cs => cs.ClassId == @class.Id && request.Date >= cs.Date && cs.ModuleId != null &&
        (cs.AttendancesWorkers != null && cs.AttendancesWorkers.Count != 0)) ?? throw new NotFoundException(nameof(ClassSession), @class.Id);

        List<Module> modules = @class.Program.Modules.OrderBy(m => m.Version).Where(m => m.TopModuleId != null || !m.SubModules.Any()).ToList();

        int totalHour = classSessionsHour.Sum(c => c.TotalHour);
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
            currentModule ??= modules.LastOrDefault();
        }
        else
        {
            currentModule = modules.FirstOrDefault();
        }

        IEnumerable<WokerDto> currentModuleWorkers = @class.ClassModulesWorkers.Where(c => c.ModuleId == currentModule.Id).Distinct(new GetModulesWorkerComparer()).Select(c => new WokerDto()
        {
            Id = c.WorkerId,
            RoleName = c.Role.Name
        });

        List<Guid> studentIds = request.Sessions
                .SelectMany(s => s.Attendances)
                .Select(a => a.StudentId)
                .ToList();

        List<Study> ClassStudiesExsist = @class.Studies.Where(c => !studentIds.Contains(c.Id)).ToList();

        IEnumerable<ClassSession> classSessions = await _unitOfWork.ClassSessionRepository
                    .GetAllAsync(c => c.Date == request.Date && c.ClassId == request.ClassId, tracking: true, "Attendances");

        foreach (UpdateAttendanceCategorySessionDto session in request.Sessions)
        {
            //foreach (ClassSession classSession in classSessions.Where(c => c.WorkerId == session.WorkerId))
            //{
            //    classSession.Status = null;
            //    classSession.WorkerId = null;
            //    classSession.ModuleId = null;
            //    classSession.Attendances = new List<Attendance>();
            //}
            foreach (ClassSession classSession in classSessions)
            {
                classSession.Status = null;
                classSession.ModuleId = null;
                classSession.Attendances = new List<Attendance>();
                classSession.AttendancesWorkers = new List<AttendanceWorker>();
            }

            ClassSession? matchingSession = classSessions.Where(cs => cs.Category == session.Category).FirstOrDefault();
            if (matchingSession == null) break;

            matchingSession.AttendancesWorkers.ToList().AddRange(session.AttendancesWorkers.Select(wa => new AttendanceWorker()
            {
                WorkerId = wa.WorkerId,
                TotalAttendanceHours = wa.IsAttendance ? matchingSession.TotalHour : 0
            }));

            matchingSession.ModuleId = request.ModuleId;
            matchingSession.Status = session.Status;

            if (session.Status != ClassSessionStatus.Cancelled)
            {
                matchingSession.Attendances = session.Attendances.Select(c => new Attendance()
                {
                    StudyId = c.StudentId,
                    Note = c.Note,
                    Status = matchingSession.TotalHour == c.TotalAttendanceHours
                                        ? StudentStatus.Attended
                                        : c.TotalAttendanceHours == 0
                                        ? StudentStatus.Absent
                                        : StudentStatus.Partial,
                    TotalAttendanceHours = c.TotalAttendanceHours
                }).ToList();
            }
            else
            {
                matchingSession.Attendances = new List<Attendance>();
                matchingSession.AttendancesWorkers = new List<AttendanceWorker>();
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
