namespace Space.Application.Handlers;

public record UpdateHolidayCommand(Guid Id, UpdateHolidayRequestDto UpdateHoliday) : IRequest<HolidayResponseDto>;

internal class UpdateHolidayCommandHandler : IRequestHandler<UpdateHolidayCommand, HolidayResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IHolidayRepository _holidayRepository;

    public UpdateHolidayCommandHandler(
        IUnitOfWork unitOfWork, IMapper mapper, IHolidayRepository holidayRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _holidayRepository = holidayRepository;
    }

    public async Task<HolidayResponseDto> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
    {
        Holiday holiday = await _holidayRepository.GetAsync(request.Id, tracking: false)
            ?? throw new NotFoundException(nameof(Holiday), request.Id);
        Holiday updateHoliday = _mapper.Map<Holiday>(request.UpdateHoliday);


        //#region Update Holiday Date
        //List<DateTime> updateHolidayDates = new();
        //for (DateOnly date = holiday.StartDate; date <= holiday.EndDate; date = date.AddDays(1))
        //{
        //    updateHolidayDates.Add(date.ToDateTime(new TimeOnly(0, 0)));
        //}
        //#endregion

        updateHoliday.Id = holiday.Id;
        _holidayRepository.Update(updateHoliday);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<HolidayResponseDto>(updateHoliday);
    }
}
