using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Space.Application.Handlers;

public class CreateModuleCommand : IRequest<GetModuleDto>
{
    public CreateModuleRequestDto Module { get; set; } = null!;
}
internal class CreateModuleHandler : IRequestHandler<CreateModuleCommand, GetModuleDto>
{
    readonly ISpaceDbContext _spaceDbContext;

    public CreateModuleHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetModuleDto> Handle(CreateModuleCommand request, CancellationToken cancellationToken)
    {
        Module module = new()
        {
            Name = request.Module.Name,
            Hours = request.Module.Hours,
            Version = request.Module.Version
        };
        if (request.Module.SubModules != null)
            module.SubModules = request.Module.SubModules.Select(sm => new Module()
            {
                Version = sm.Version,
                Name = sm.Name,
                Hours = sm.Hours

            }).ToList();

        EntityEntry<Module> entry = await _spaceDbContext.Modules.AddAsync(module, cancellationToken);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);

        Module newModule = entry.Entity;
        return new GetModuleDto()
        {
            Hours = newModule.Hours,
            Id = newModule.Id,
            Name = newModule.Name,
            SubModules = newModule.SubModules?.Select(sm => new SubModuleDto()
            {
                TopModuleId = newModule.Id,
                Id = sm.Id,
                Name = sm.Name,
                Hours = sm.Hours,
                Version = sm.Version
            })
        };
    }
}
