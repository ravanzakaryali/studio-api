namespace Space.Application.Handlers;

public record DeleteRoomCommand(Guid Id) : IRequest<GetRoomResponseDto>;

internal class DeleteRoomCommandHanler : IRequestHandler<DeleteRoomCommand, GetRoomResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IRoomRepository _roomRepository;

    public DeleteRoomCommandHanler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRoomRepository roomRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public async Task<GetRoomResponseDto> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        Room? room = await _roomRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Room), request.Id);
        _roomRepository.Remove(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetRoomResponseDto>(room);
    }
}
