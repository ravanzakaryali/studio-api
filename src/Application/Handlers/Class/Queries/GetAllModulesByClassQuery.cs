using Microsoft.AspNetCore.Identity;

namespace Space.Application.Handlers;

public record GetAllModulesByClassQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetModuleDto>>;


internal class GetAllModulesByClassQueryHandler : IRequestHandler<GetAllModulesByClassQuery, IEnumerable<GetModuleDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ICurrentUserService _currentUserService;
    readonly UserManager<User> _userManager;

    public GetAllModulesByClassQueryHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task<IEnumerable<GetModuleDto>> Handle(GetAllModulesByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _unitOfWork.ClassRepository
            .GetAsync(request.Id, tracking: false, "Program.Modules.SubModules") ??
            throw new NotFoundException(nameof(Class), request.Id);


        IEnumerable<ClassSession> classSessions = await _unitOfWork.ClassSessionRepository
            .GetAllAsync(cs => cs.ClassId == @class.Id && request.Date >= cs.Date && cs.Category != ClassSessionCategory.Lab) ?? throw new NotFoundException(nameof(ClassSession), request.Id);

        List<Module> modules = @class.Program.Modules
            .OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null)
            .Where(m => m.TopModuleId != null || !m.SubModules.Any())
            .ToList();

        int totalHour = classSessions
            .Sum(c => c.TotalHour);

        List<Module> modulesResponse = new();
        if (totalHour > 0)
        {
            double totalHourModule = 0;

            for (int i = 0; i < modules.Count; i++)
            {
                totalHourModule += modules[i].Hours;
                if (totalHourModule >= totalHour)
                {
                    modulesResponse.Add(modules[i]);
                    if (i == 0 && modules.Count > 1)
                    {
                        modulesResponse.Add(modules[i + 1]);
                    }
                    else if (modules.Count == i - 1 && modules.Count > 1)
                    {
                        modulesResponse.Add(modules[i - 1]);
                    }
                    else
                    {
                        if (i != modules.Count - 1)
                        {
                            modulesResponse.Add(modules[i + 1]);
                        }
                        modulesResponse.Add(modules[i - 1]);
                    }
                    break;
                }
            }
            if (modulesResponse.Count == 0)
            {
                modulesResponse = modules.TakeLast(2).ToList();
            }
        }
        else
        {
            modulesResponse = modules
                                .Take(2)
                                .ToList();
        }

        //User user = await _userManager.FindByIdAsync(_currentUserService.UserId.ToString())
        //    ?? throw new AutheticationException();
        //List<Guid> ids = modulesResponse.Select(m => m.Id).ToList();

        //if (await _unitOfWork.ClassModulesWorkerRepository.IsWorkerExist(user.Id, ids))
        //{
        //}
        return _mapper.Map<IEnumerable<GetModuleDto>>(modulesResponse.OrderBy(c => c.Version));


    }
}
