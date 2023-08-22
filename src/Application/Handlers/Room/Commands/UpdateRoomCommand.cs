namespace Space.Application.Handlers;

public record UpdateRoomCommand(Guid Id, UpdateRoomRequestDto UpdateRoom) : IRequest<GetRoomResponseDto>;

internal class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, GetRoomResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public UpdateRoomCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetRoomResponseDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        Room? room = await _unitOfWork.RoomRepository.GetAsync(request.Id, tracking: false)
            ?? throw new NotFoundException(nameof(Room),request.Id);
        Room newRoom = _mapper.Map<Room>(request.UpdateRoom);
        newRoom.Id = request.Id;
        _unitOfWork.RoomRepository.Update(newRoom);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetRoomResponseDto>(newRoom);
    }
}
