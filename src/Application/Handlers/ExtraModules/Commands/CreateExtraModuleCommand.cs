namespace Space.Application.Handlers;
public class CreateExtraModuleCommand : IRequest<GetExtraModuleDto>
{
    public string Name { get; set; } = null!;
    public int Hours { get; set; }
    public string Version { get; set; } = null!;
    public int ProgramId { get; set; }
}

//mapper dont use
//dbContext name is ISpaceDbContext
internal class CreateExtraModuleHandler : IRequestHandler<CreateExtraModuleCommand, GetExtraModuleDto>
{
    private readonly ISpaceDbContext _context;

    public CreateExtraModuleHandler(ISpaceDbContext context)
    {
        _context = context;
    }

    public async Task<GetExtraModuleDto> Handle(CreateExtraModuleCommand request, CancellationToken cancellationToken)
    {
        ExtraModule entity = new()
        {
            Name = request.Name,
            Hours = request.Hours,
            Version = request.Version,
            ProgramId = request.ProgramId
        };
        _context.ExtraModules.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return new GetExtraModuleDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Hours = entity.Hours,
            Version = entity.Version
        };
    }
}