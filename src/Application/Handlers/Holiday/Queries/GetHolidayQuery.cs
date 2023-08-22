namespace Space.Application.Handlers;

public record GetHolidayQuery(Guid Id) : IRequest<HolidayResponseDto>;

internal class GetHolidayQueryHandler : IRequestHandler<GetHolidayQuery, HolidayResponseDto>
{
    readonly IMapper _mapper;
    readonly IUnitOfWork _unitOfWork;

    public GetHolidayQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<HolidayResponseDto> Handle(GetHolidayQuery request, CancellationToken cancellationToken)
    {
        Holiday holiday = await _unitOfWork.HolidayRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Holiday),request.Id);
        return _mapper.Map<HolidayResponseDto>(holiday);
    }
}
