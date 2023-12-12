using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Space.Application.Handlers.Commands;

public record CreateClassCommand(
        string Name,
        int ProgramId,
        int SessionId,
        int? RoomId) : IRequest<GetWithIncludeClassResponseDto>;
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

        if (request.RoomId is not null)
        {
            Room room = await _spaceDbContext.Rooms.FindAsync(request.RoomId) ??
                throw new NotFoundException(nameof(Room), request.RoomId);
        }

        EntityEntry<Class> createEntity = await _spaceDbContext.Classes.AddAsync(_mapper.Map<Class>(request), cancellationToken);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWithIncludeClassResponseDto>(createEntity.Entity);
    }
}
