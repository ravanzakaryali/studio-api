namespace Space.Application.Handlers;

public record GetAllRoomQuery : IRequest<IEnumerable<GetRoomResponseDto>>;

internal class GetAllRoomQueryHandler : IRequestHandler<GetAllRoomQuery, IEnumerable<GetRoomResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IRoomRepository _roomRepository;

    public GetAllRoomQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRoomRepository roomRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<GetRoomResponseDto>> Handle(GetAllRoomQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<GetRoomResponseDto>>(await _roomRepository.GetAllAsync());
    }
}
