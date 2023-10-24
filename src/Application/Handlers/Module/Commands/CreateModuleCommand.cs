using Space.Domain.Entities;
using System.Collections;

namespace Space.Application.Handlers.Commands;

public record CreateModuleCommand(Guid ProgramId, IEnumerable<ModuleDto> Modules) : IRequest<IEnumerable<GetModuleDto>>;
internal class CreateModuleCommandHandler : IRequestHandler<CreateModuleCommand, IEnumerable<GetModuleDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IProgramRepository _programRepository;
    readonly IModuleRepository _moduleRepository;

    public CreateModuleCommandHandler(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IProgramRepository programRepository,
        IModuleRepository moduleRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _programRepository = programRepository;
        _moduleRepository = moduleRepository;
    }

    public async Task<IEnumerable<GetModuleDto>> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
    {
        List<Module> modules = _mapper.Map<List<Module>>(request.Modules)
              .Select(m =>
              {
                  m.ProgramId = request.ProgramId;
                  if (m.SubModules?.Count != 0)
                  {
                      m.Hours = m.SubModules!.Sum(subModule => subModule.Hours);
                      m.SubModules = m.SubModules!.Select(sm =>
                      {
                          sm.ProgramId = request.ProgramId;
                          return sm;
                      }).ToList();
                      return m;
                  }
                  else
                  {
                      return m;
                  }
              }).ToList();

        Program? program = await _programRepository.GetAsync(a => a.Id == request.ProgramId, includeProperties: "Modules")
                        ?? throw new NotFoundException(nameof(Program), request.ProgramId);

        if (program.Modules != null)
        {
            foreach (Module module in program.Modules)
            {
                _moduleRepository.Remove(module);
            }
        }

        if (program.TotalHours != modules.Sum(m => m.Hours))
        {
            IEnumerable<ValidationFailure> failure = new List<ValidationFailure>()
            {
                new ValidationFailure("TotalHours",$"The total {program.TotalHours} hours of the '{nameof(Program)}' must be equal to the sum of the hours of the '{nameof(Program)}'")
            };
            throw new ValidationException("Program hours error", failure);
        }
        List<string> moduleNames = modules.Select(a => a.Name).ToList();
        List<string> subModulesNames = modules.SelectMany(a => a.SubModules!.Select(a => a.Name)).ToList();
        moduleNames.AddRange(subModulesNames);

        //Module? modulesDb = await _unitOfWork.ModuleRepository.GetAsync(a => moduleNames.Contains(a.Name) && a.ProgramId == request.ProgramId);
        //if (modulesDb != null) throw new Exception("Module already exsist");

        await _moduleRepository.AddRangeAsync(modules);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<IEnumerable<GetModuleDto>>(modules);
    }
}

