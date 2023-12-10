
namespace Space.Application.Handlers;

public class CreateHeldModulesByClassCommand : IRequest
{
    public Guid ClassId { get; set; }
    public IEnumerable<CreateAttendanceModuleRequestDto> HeldModules { get; set; } = null!;
}
internal class CreateHeldModulesByClassHandler : IRequestHandler<CreateHeldModulesByClassCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public CreateHeldModulesByClassHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateHeldModulesByClassCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes.Where(c => c.Id == request.ClassId).FirstOrDefaultAsync()
            ?? throw new NotFoundException(nameof(Class),request.ClassId);
    }
}
