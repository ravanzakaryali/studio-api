namespace Space.Application.Handlers;

public record GetHolidayQuery(Guid Id) : IRequest<HolidayResponseDto>;

internal class GetHolidayQueryHandler : IRequestHandler<GetHolidayQuery, HolidayResponseDto>
{
    readonly IMapper _mapper;
    readonly IUnitOfWork _unitOfWork;
    readonly IHolidayRepository _holidayRepository;

    public GetHolidayQueryHandler(
        IMapper mapper, 
        IUnitOfWork unitOfWork, 
        IHolidayRepository holidayRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _holidayRepository = holidayRepository;
    }

    public async Task<HolidayResponseDto> Handle(GetHolidayQuery request, CancellationToken cancellationToken)
    {
        Holiday holiday = await _holidayRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Holiday), request.Id);
        return _mapper.Map<HolidayResponseDto>(holiday);
    }
}
