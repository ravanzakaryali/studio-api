namespace Space.Application.Handlers;

public record DeleteRoomCommand(Guid Id) : IRequest<GetRoomResponseDto>;

internal class DeleteRoomCommandHanler : IRequestHandler<DeleteRoomCommand, GetRoomResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public DeleteRoomCommandHanler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetRoomResponseDto> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        Room? room = await _unitOfWork.RoomRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Room),request.Id);
        _unitOfWork.RoomRepository.Remove(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetRoomResponseDto>(room);
    }
}
