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
    readonly IUnitOfWork _unitOfWork;

    public CreateClassCommandHandler(
        ISpaceDbContext spaceDbContext, IUnitOfWork unitOfWork)
    {
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
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

        Class @class = new()
        {
            StartDate = request.StartDate,
            Program = program,
            Session = session,
            Project = project,
            Room = room,
            VitrinWeek = request.Week,
            VitrinDate = request.StartDate.AddDays(request.Week * 7),
        };

        DateOnly endDate = await _unitOfWork.ClassService.EndDateCalculationAsync(@class);
        string name = await _unitOfWork.ClassService.GenerateClassName(@class);
        @class.EndDate = endDate;
        @class.Name = name;
        await _spaceDbContext.Classes.AddAsync(@class);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);

        return new GetWithIncludeClassResponseDto
        {
            Id = @class.Id,
            Name = @class.Name,
            Program = new GetProgramResponseDto()
            {
                Id = program.Id,
                Name = program.Name,
            },
            Room = new GetRoomResponseDto()
            {
                Id = room.Id,
                Name = room.Name,
            },
            Session = new GetSessionWithDetailsResponseDto()
            {
                Id = session.Id,
                Name = session.Name,
                Details = session.Details.Select(d => new GetDetailsResponseDto
                {
                    Id = d.Id,
                    DayOfWeek = d.DayOfWeek,
                    TotalHours = d.TotalHours,
                }).ToList(),
            },
        };
    }

}
