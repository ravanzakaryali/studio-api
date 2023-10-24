namespace Space.Application.Handlers;

public record GetAllHolidayQuery : IRequest<IEnumerable<HolidayResponseDto>>;
internal class GetAllHolidayQueryHandler : IRequestHandler<GetAllHolidayQuery, IEnumerable<HolidayResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IHolidayRepository _holidayRepository;

    public GetAllHolidayQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHolidayRepository holidayRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _holidayRepository = holidayRepository;
    }

    public async Task<IEnumerable<HolidayResponseDto>> Handle(GetAllHolidayQuery request, CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<HolidayResponseDto>>(await _holidayRepository.GetAllAsync());
}
