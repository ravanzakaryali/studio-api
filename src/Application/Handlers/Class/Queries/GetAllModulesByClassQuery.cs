namespace Space.Application.Handlers;

public record GetAllModulesByClassQuery(Guid Id, DateOnly Date) : IRequest<IEnumerable<GetModuleDto>>;


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
            .Include(c => c.ClassModulesWorkers)
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .ThenInclude(c => c.SubModules)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        List<ClassTimeSheet> timeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == @class.Id && request.Date >= c.Date && c.Category != ClassSessionCategory.Lab)
            .ToListAsync(cancellationToken: cancellationToken);

        List<Module> modules = @class.Program.Modules
            .OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null)
            .Where(m => m.TopModuleId != null || m.SubModules!.Any())
            .ToList();

        int totalHour = timeSheets
            .Sum(c => c.TotalHours);

        ClassModulesWorker? currentModuleWorker = @class.ClassModulesWorkers
            .FirstOrDefault(c => c.StartDate >= request.Date && c.EndDate <= request.Date)
                ?? throw new NotFoundException(nameof(ClassModulesWorker), request.Date);

        int currentModuleIndex = modules.FindIndex(c => c.Id == currentModuleWorker.Id);
        List<Module> modulesResponse = new();

        if (currentModuleIndex >= 0)
            AddModuleToResponse(modules, currentModuleIndex, modulesResponse);

        return _mapper.Map<IEnumerable<GetModuleDto>>(modulesResponse.OrderBy(c => c.Version));
    }
    void AddModuleToResponse(List<Module> modulesList, int index, List<Module> responseList)
    {
        int[] indices = { index, index - 1, index + 1 };

        foreach (var i in indices)
        {
            if (i >= 0 && i < modulesList.Count)
            {
                responseList.Add(modulesList[i]);
            }
        }
    }
}
