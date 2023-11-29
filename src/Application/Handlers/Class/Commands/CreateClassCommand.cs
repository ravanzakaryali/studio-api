using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Space.Application.Handlers.Commands;

public record CreateClassCommand(
        string Name,
        Guid ProgramId,
        Guid SessionId,
        Guid? RoomId) : IRequest<GetWithIncludeClassResponseDto>;
internal class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, GetWithIncludeClassResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateClassCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetWithIncludeClassResponseDto> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        Program? program = await _spaceDbContext.Programs.FindAsync(request.ProgramId) ??
            throw new NotFoundException(nameof(Program), request.ProgramId);

        Session session = await _spaceDbContext.Sessions.FindAsync(request.SessionId) ??
            throw new NotFoundException(nameof(Session), request.SessionId);

        if (request.RoomId != null)
        {
            Guid roomIdNonNullable = request.RoomId ?? Guid.Empty;
            Room room = await _spaceDbContext.Rooms.FindAsync(roomIdNonNullable) ??
                throw new NotFoundException(nameof(Room), roomIdNonNullable);
        }

        EntityEntry<Class> createEntity = await _spaceDbContext.Classes.AddAsync(_mapper.Map<Class>(request), cancellationToken);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWithIncludeClassResponseDto>(createEntity.Entity);
    }
}
