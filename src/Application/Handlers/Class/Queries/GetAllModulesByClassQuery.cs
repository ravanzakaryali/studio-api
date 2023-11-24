using Microsoft.AspNetCore.Identity;

namespace Space.Application.Handlers;

public record GetAllModulesByClassQuery(Guid Id, DateTime Date) : IRequest<IEnumerable<GetModuleDto>>;


internal class GetAllModulesByClassQueryHandler : IRequestHandler<GetAllModulesByClassQuery, IEnumerable<GetModuleDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;
    readonly ICurrentUserService _currentUserService;
    readonly UserManager<User> _userManager;

    public GetAllModulesByClassQueryHandler(IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        UserManager<User> userManager,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _userManager = userManager;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetModuleDto>> Handle(GetAllModulesByClassQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .ThenInclude(c => c.SubModules)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.Id);

        List<ClassTimeSheet> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => c.ClassId == @class.Id && request.Date >= c.Date && c.Category != ClassSessionCategory.Lab).ToListAsync();

        List<Module> modules = @class.Program.Modules
            .OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null)
            .Where(m => m.TopModuleId != null || m.SubModules!.Any())
            .ToList();

        int totalHour = classSessions
            .Sum(c => c.TotalHour);


        //Todo: Code Review 
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

        return _mapper.Map<IEnumerable<GetModuleDto>>(modulesResponse.OrderBy(c => c.Version));


    }
}
