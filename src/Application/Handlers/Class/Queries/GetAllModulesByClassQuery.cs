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
            .Include(c => c.ClassModulesWorkers)
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .ThenInclude(c => c.SubModules)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        DateOnly requestDate = DateOnly.FromDateTime(request.Date);

        List<ClassTimeSheet> timeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == @class.Id && requestDate >= c.Date && c.Category != ClassSessionCategory.Lab)
            .ToListAsync(cancellationToken: cancellationToken);

        List<Module> modules = @class.Program.Modules
            .OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null)
            .Where(m => m.TopModuleId != null)
            .ToList();

        int totalHour = timeSheets
            .Sum(c => c.TotalHours);

        //2023-12-12
        //2023-12-14
        //2023-12-24
        ClassModulesWorker? currentModuleWorker = @class.ClassModulesWorkers
            .FirstOrDefault(c => c.StartDate <= requestDate && c.EndDate >= requestDate)
                ?? throw new NotFoundException(nameof(ClassModulesWorker), requestDate);

        int currentModuleIndex = modules.FindIndex(c => c.Id == currentModuleWorker.ModuleId);
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
