namespace Space.Application.Handlers;

public record GetHolidayQuery(int Id) : IRequest<HolidayResponseDto>;

internal class GetHolidayQueryHandler : IRequestHandler<GetHolidayQuery, HolidayResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetHolidayQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<HolidayResponseDto> Handle(GetHolidayQuery request, CancellationToken cancellationToken)
    {
        Holiday holiday = await _spaceDbContext.Holidays.FindAsync(request.Id)
            ?? throw new NotFoundException(nameof(Holiday), request.Id);
        return _mapper.Map<HolidayResponseDto>(holiday);
    }
}
