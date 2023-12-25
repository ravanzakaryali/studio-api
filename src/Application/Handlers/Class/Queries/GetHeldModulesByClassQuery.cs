
namespace Space.Application.Handlers;

public class GetHeldModulesByClassQuery : IRequest<IEnumerable<GetHeldModulesDto>>
{
    public int Id { get; set; }
}
internal class GetHeldModulesByClassHandler : IRequestHandler<GetHeldModulesByClassQuery, IEnumerable<GetHeldModulesDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetHeldModulesByClassHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetHeldModulesDto>> Handle(GetHeldModulesByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
        ClassTimeSheet classTimeSheet = await _spaceDbContext.ClassTimeSheets
            .Include(c => c.HeldModules)
            .ThenInclude(c => c.Module)
            .Where(cs => cs.ClassId == @class.Id && cs.Date == dateNow && cs.Category == ClassSessionCategory.Theoric)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(ClassTimeSheet), request.Id);

        return classTimeSheet.HeldModules
        .OrderBy(m => Version.TryParse(m.Module.Version, out var parsedVersion) ? parsedVersion : null)
        .Select(c => new GetHeldModulesDto()
        {
            TotalHours = c.TotalHours,
            Name = c.Module.Name,
            Id = c.ModuleId,
            Version = c.Module.Version,
        });
    }
}
