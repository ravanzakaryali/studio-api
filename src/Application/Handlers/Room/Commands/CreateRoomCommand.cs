namespace Space.Application.Handlers;


public record CreateRoomCommand(string Name, int Capacity, RoomType Type) : IRequest<CreateRoomResponseDto>;

internal class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, CreateRoomResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IRoomRepository _roomRepository;

    public CreateRoomCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRoomRepository roomRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public async Task<CreateRoomResponseDto> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        Room? room = await _roomRepository.GetAsync(a => a.Name == request.Name);
        if (room is not null) throw new AlreadyExistsException(nameof(Room), request.Name);
        Room newRoom = _mapper.Map<Room>(request);
        await _roomRepository.AddAsync(newRoom);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CreateRoomResponseDto>(newRoom);
    }
}
