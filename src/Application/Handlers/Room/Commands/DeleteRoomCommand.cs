namespace Space.Application.Handlers;

public record DeleteRoomCommand(Guid Id) : IRequest<GetRoomResponseDto>;

internal class DeleteRoomCommandHanler : IRequestHandler<DeleteRoomCommand, GetRoomResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public DeleteRoomCommandHanler(
        IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetRoomResponseDto> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        Room? room = await _spaceDbContext.Rooms.FindAsync(request.Id)
            ?? throw new NotFoundException(nameof(Room), request.Id);
        room.IsDeleted = true;
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<GetRoomResponseDto>(room);
    }
}
