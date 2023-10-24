namespace Space.Application.Handlers;

public record UpdateRoomCommand(Guid Id, UpdateRoomRequestDto UpdateRoom) : IRequest<GetRoomResponseDto>;

internal class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, GetRoomResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IRoomRepository _roomRepository;
    public UpdateRoomCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper, 
        IRoomRepository roomRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public async Task<GetRoomResponseDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        Room? room = await _roomRepository.GetAsync(request.Id, tracking: false)
            ?? throw new NotFoundException(nameof(Room),request.Id);
        Room newRoom = _mapper.Map<Room>(request.UpdateRoom);
        newRoom.Id = request.Id;
        _roomRepository.Update(newRoom);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetRoomResponseDto>(newRoom);
    }
}
