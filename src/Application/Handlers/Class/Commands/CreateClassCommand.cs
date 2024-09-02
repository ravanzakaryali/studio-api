using Microsoft.EntityFrameworkCore.ChangeTracking;
namespace Space.Application.Handlers;


public class CreateClassCommand : IRequest<GetWithIncludeClassResponseDto>
{
    public DateOnly StartDate { get; set; }
    public int Week { get; set; }
    public int ProjectId { get; set; }
    public int ProgramId { get; set; }
    public int RoomId { get; set; }
    public int SessionId { get; set; }
}
internal class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, GetWithIncludeClassResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;

    public CreateClassCommandHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetWithIncludeClassResponseDto> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        Program? program = await _spaceDbContext.Programs.FindAsync(request.ProgramId) ??
            throw new NotFoundException(nameof(Program), request.ProgramId);

        Session session = await _spaceDbContext.Sessions.FindAsync(request.SessionId) ??
            throw new NotFoundException(nameof(Session), request.SessionId);

        Project project = await _spaceDbContext.Projects.FindAsync(request.ProjectId) ??
            throw new NotFoundException(nameof(Project), request.ProjectId);

        Room room = await _spaceDbContext.Rooms.FindAsync(request.RoomId) ??
            throw new NotFoundException(nameof(Room), request.RoomId);

        throw new NotImplementedException();
    }
}
