namespace Space.Application.Handlers;

public record UpdateRoomCommand(int Id, UpdateRoomRequestDto UpdateRoom) : IRequest<GetRoomResponseDto>;

internal class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, GetRoomResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;
    public UpdateRoomCommandHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetRoomResponseDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        Room? room = await _spaceDbContext.Rooms.FindAsync(request.Id)
            ?? throw new NotFoundException(nameof(Room), request.Id);
        Room newRoom = _mapper.Map<Room>(request.UpdateRoom);
        newRoom.Id = request.Id;
        _spaceDbContext.Rooms.Update(newRoom);
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<GetRoomResponseDto>(newRoom);
    }
}
