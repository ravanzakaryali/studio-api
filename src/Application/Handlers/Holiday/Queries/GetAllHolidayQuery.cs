namespace Space.Application.Handlers;

public record GetAllHolidayQuery : IRequest<IEnumerable<HolidayResponseDto>>;
internal class GetAllHolidayQueryHandler : IRequestHandler<GetAllHolidayQuery, IEnumerable<HolidayResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetAllHolidayQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<HolidayResponseDto>> Handle(GetAllHolidayQuery request, CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<HolidayResponseDto>>(await _spaceDbContext.Holidays.ToListAsync());
}
