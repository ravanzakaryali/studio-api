namespace Space.Application.Handlers.Commands;

public record UpdateClassCommand(
    int Id,
    string Name,
    int SessionId,
    int ProgramId,
    int? RoomId
    ) : IRequest<GetClassResponseDto>;
internal class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, GetClassResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;


    public UpdateClassCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetClassResponseDto> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Class), request.Id);
        Program program = await _spaceDbContext.Programs.FindAsync(request.ProgramId) ??
            throw new NotFoundException(nameof(Program), request.ProgramId);

        Session session = await _spaceDbContext.Sessions.FindAsync(request.SessionId) ??
            throw new NotFoundException(nameof(Session), request.SessionId);
        if (request.RoomId == null)
        {
            Room rooo = await _spaceDbContext.Rooms.FindAsync(request.RoomId) ??
                throw new NotFoundException(nameof(Room), request.RoomId);
        }

        Class newClass = _mapper.Map<Class>(request);

        newClass.Id = @class.Id;

        _spaceDbContext.Classes.Update(newClass);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetClassResponseDto>(newClass);
    }
}
