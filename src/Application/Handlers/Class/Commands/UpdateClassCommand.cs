namespace Space.Application.Handlers.Commands;

public record UpdateClassCommand(
    Guid Id,
    string Name,
    Guid SessionId,
    Guid ProgramId,
    Guid? RoomId
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
            Guid roomIdNonNullable = request.RoomId ?? Guid.Empty;
            Room rooo = await _spaceDbContext.Rooms.FindAsync(roomIdNonNullable) ??
                throw new NotFoundException(nameof(Room), roomIdNonNullable);
        }

        Class newClass = _mapper.Map<Class>(request);

        newClass.Id = @class.Id;

        _spaceDbContext.Classes.Update(newClass);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetClassResponseDto>(newClass);
    }
}
