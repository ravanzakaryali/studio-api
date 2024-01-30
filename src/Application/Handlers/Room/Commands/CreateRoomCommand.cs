namespace Space.Application.Handlers;

public record CreateRoomCommand(string Name, int Capacity, RoomType Type) : IRequest<CreateRoomResponseDto>;

internal class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, CreateRoomResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateRoomCommandHandler(
        IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<CreateRoomResponseDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        Room? room = await _spaceDbContext.Rooms.Where(a => a.Name == request.Name).FirstOrDefaultAsync();
        if (room is not null) throw new AlreadyExistsException(nameof(Room), request.Name);
        Room newRoom = _mapper.Map<Room>(request);
        await _spaceDbContext.Rooms.AddAsync(newRoom);
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<CreateRoomResponseDto>(newRoom);
    }
}
