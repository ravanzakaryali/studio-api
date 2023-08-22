using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace Space.Application.Handlers;

public record GetClassQuery(Guid Id) : IRequest<IEnumerable<GetClassModuleResponseDto>>;

internal class GetClassQueryHandler : IRequestHandler<GetClassQuery, IEnumerable<GetClassModuleResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetClassQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetClassModuleResponseDto>> Handle(GetClassQuery request, CancellationToken cancellationToken)
    {

        Class? @class = await _unitOfWork.ClassRepository.GetAsync(request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);

        IEnumerable<Module> modules = await _unitOfWork.ModuleRepository.GetAllAsync(m => m.ProgramId == @class.ProgramId && m.TopModuleId == null, tracking: false, "SubModules");

        IEnumerable<ClassModulesWorker> classModulesWorkers = await _unitOfWork.ClassModulesWorkerRepository.GetAllAsync(
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