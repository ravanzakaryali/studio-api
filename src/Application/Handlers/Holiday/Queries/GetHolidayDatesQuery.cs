namespace Space.Application.Handlers;

public class GetHolidayDatesQuery : IRequest<IEnumerable<GetHolidayDatesDto>>
{
}

internal class GetHolidayDatesQueryHandler : IRequestHandler<GetHolidayDatesQuery, IEnumerable<GetHolidayDatesDto>>
{
    readonly IUnitOfWork _unitOfWork;

    public GetHolidayDatesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetHolidayDatesDto>> Handle(GetHolidayDatesQuery request, CancellationToken cancellationToken)
    {
        List<DateTime> dates = await _unitOfWork.HolidayService.GetDatesAsync();
        return dates.Select(c => new GetHolidayDatesDto()
        {
            Date = c
        });
    }
}
