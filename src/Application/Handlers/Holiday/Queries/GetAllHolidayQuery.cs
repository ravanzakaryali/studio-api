namespace Space.Application.Handlers;

public record GetAllHolidayQuery : IRequest<IEnumerable<HolidayResponseDto>>;
internal class GetAllHolidayQueryHandler : IRequestHandler<GetAllHolidayQuery, IEnumerable<HolidayResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetAllHolidayQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<HolidayResponseDto>> Handle(GetAllHolidayQuery request, CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<HolidayResponseDto>>(await _unitOfWork.HolidayRepository.GetAllAsync());
}
