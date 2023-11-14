namespace Space.Application.Handlers;

public class GetHolidayDatesQuery : IRequest<IEnumerable<GetHolidayDatesDto>>
{
}

internal class GetHolidayDatesQueryHandler : IRequestHandler<GetHolidayDatesQuery, IEnumerable<GetHolidayDatesDto>>
{
    readonly IHolidayRepository _holidayRepository;

    public GetHolidayDatesQueryHandler(IHolidayRepository holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }

    public async Task<IEnumerable<GetHolidayDatesDto>> Handle(GetHolidayDatesQuery request, CancellationToken cancellationToken)
    {
        List<DateTime> dates = await _holidayRepository.GetDatesAsync();
        return dates.Select(c => new GetHolidayDatesDto()
        {
            Date = c
        });
    }
}
