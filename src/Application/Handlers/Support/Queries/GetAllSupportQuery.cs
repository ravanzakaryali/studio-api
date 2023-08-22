namespace Space.Application.Handlers;

public record GetAllSupportQuery : IRequest<IEnumerable<GetSupportResponseDto>>;

internal class GetAllSupportQueryHandler : IRequestHandler<GetAllSupportQuery, IEnumerable<GetSupportResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IStorageService _storage;
    readonly IMapper _mapper;

    public GetAllSupportQueryHandler(IUnitOfWork unitOfWork, IStorageService storage, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _storage = storage;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetSupportResponseDto>> Handle(GetAllSupportQuery request, CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<GetSupportResponseDto>>(await _unitOfWork.SupportRepository.GetAllAsync(predicate: null, tracking: false, "SupportImages", "User"));
}
