namespace Space.Application.Handlers;

public record GetAllRoomQuery : IRequest<IEnumerable<GetRoomResponseDto>>;

internal class GetAllRoomQueryHandler : IRequestHandler<GetAllRoomQuery, IEnumerable<GetRoomResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetAllRoomQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetRoomResponseDto>> Handle(GetAllRoomQuery request, CancellationToken cancellationToken)
    {
        return _mapper.Map<IEnumerable<GetRoomResponseDto>>(await _unitOfWork.RoomRepository.GetAllAsync());
    }
}
