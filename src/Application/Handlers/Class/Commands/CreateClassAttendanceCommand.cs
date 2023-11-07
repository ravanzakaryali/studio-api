using Space.Application.Abstractions;
using Space.Domain.Entities;

namespace Space.Application.Handlers;

public class CreateClassAttendanceCommand : IRequest
{
    public Guid ClassId { get; set; }
    public Guid ModuleId { get; set; }
    public DateTime Date { get; set; }
    public ICollection<UpdateAttendanceCategorySessionDto> Sessions { get; set; } = null!;
}
internal class CreateClassAttendanceCommandHandler : IRequestHandler<CreateClassAttendanceCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IClassRepository _classRepository;
    readonly IModuleRepository _moduleRepository;
    readonly IClassSessionRepository _classSessionRepository;

    public CreateClassAttendanceCommandHandler(
        IUnitOfWork unitOfWork,
        IClassRepository classRepository,
        IModuleRepository moduleRepository,
        IClassSessionRepository classSessionRepository)
    {
        _unitOfWork = unitOfWork;
        _classRepository = classRepository;
        _moduleRepository = moduleRepository;
        _classSessionRepository = classSessionRepository;
    }

    public async Task Handle(CreateClassAttendanceCommand request, CancellationToken cancellationToken)
    {

        Class @class = await _classRepository.GetAsync(
                                                                  request.ClassId,
                                                                  tracking: false,
                                                                  "Studies",
                                                                  "Program.Modules.SubModules",
                                                                  "ClassModulesWorkers.Worker",
                                                                  "ClassModulesWorkers.Role") ??
                                                                        throw new NotFoundException(nameof(Class), request.ClassId);

        Module module = await _moduleRepository.GetAsync(
                                                                   request.ModuleId,
                                                                   tracking: false) ??
                                                                        throw new NotFoundException(nameof(Module), request.ModuleId);

        IEnumerable<ClassSession> classSessionsHour = await _classSessionRepository.GetAllAsync(cs =>
                                                                        cs.ClassId == @class.Id &&
                                                                        request.Date >= cs.Date &&
                                                                        cs.ModuleId != null &&
                                                                        (cs.AttendancesWorkers != null &&
                                                                        cs.AttendancesWorkers.Count != 0)) ??
                                                                           throw new NotFoundException(nameof(ClassSession), @class.Id);

        List<Module> modules = @class.Program.Modules
                                                    .OrderBy(m => m.Version)
                                                    .Where(m => m.TopModuleId != null ||
                                                    !m.SubModules!.Any()).ToList();


        Module? currentModule = await _moduleRepository.GetCurrentModuleAsync(@class, request.Date);

        IEnumerable<WokerDto> currentModuleWorkers = @class.ClassModulesWorkers
                                                                                .Where(c => c.ModuleId == currentModule?.Id)
                                                                                .Distinct(new GetModulesWorkerComparer())
                                                                                .Select(c => new WokerDto()
                                                                                {
                                                                                    Id = c.WorkerId,
                                                                                    RoleName = c.Role?.Name
                                                                                });
        List<Guid> studentIds = request.Sessions
                .SelectMany(s => s.Attendances)
                .Select(a => a.StudentId)
                .ToList();

        List<Study> ClassStudiesExsist = @class.Studies.Where(c => !studentIds.Contains(c.Id)).ToList();

        IEnumerable<ClassSession> classSessions = await _classSessionRepository
                    .GetAllAsync(c => c.Date == request.Date &&
                                      c.ClassId == request.ClassId,
                                      tracking: true,
                                      "Attendances",
                                      "AttendancesWorkers");

        await _classSessionRepository.GenerateAttendanceAsync(request.Sessions, classSessions, request.ModuleId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
