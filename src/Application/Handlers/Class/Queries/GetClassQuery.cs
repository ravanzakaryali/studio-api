using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace Space.Application.Handlers;

public record GetClassQuery(Guid Id) : IRequest<IEnumerable<GetClassModuleResponseDto>>;

internal class GetClassQueryHandler : IRequestHandler<GetClassQuery, IEnumerable<GetClassModuleResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ICurrentUserService _currentUserService;
    readonly IClassRepository _classRepository;
    readonly IModuleRepository _moduleRepository;
    readonly IClassModulesWorkerRepository _classModulesWorkerRepository;

    public GetClassQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService
        currentUserService,
        IClassRepository classRepository,
        IModuleRepository moduleRepository,
        IClassModulesWorkerRepository classModulesWorkerRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _classRepository = classRepository;
        _moduleRepository = moduleRepository;
        _classModulesWorkerRepository = classModulesWorkerRepository;
    }

    public async Task<IEnumerable<GetClassModuleResponseDto>> Handle(GetClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _classRepository.GetAsync(request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);

        IEnumerable<Module> modules = await _moduleRepository.GetAllAsync(m => m.ProgramId == @class.ProgramId && m.TopModuleId == null, tracking: false, "SubModules");

        IEnumerable<ClassModulesWorker> classModulesWorkers = await _classModulesWorkerRepository.GetAllAsync(
            cmw => cmw.ClassId == @class.Id, tracking: false, "Role", "Worker");

        IEnumerable<GetClassModuleResponseDto> response = modules.Select(m => new GetClassModuleResponseDto()
        {
            ClassName = @class.Name,
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
            }),
            Workers = _mapper.Map<ICollection<GetWorkerForClassDto>>(classModulesWorkers.Where(cmw => m.SubModules.Any(s => s.Id == cmw.ModuleId)).Distinct(new GetWorkerForClassDtoComparer()))
        });
        return response;
    }
}

public class GetWorkerForClassDtoComparer : IEqualityComparer<ClassModulesWorker>
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