namespace Space.Application.Handlers;

public record DeleteHolidayCommand(Guid Id) : IRequest<HolidayResponseDto>;

internal class DeleteHolidayCommandHandler : IRequestHandler<DeleteHolidayCommand, HolidayResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IHolidayRepository _holidayRepository;

    public DeleteHolidayCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<HolidayResponseDto> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
    {
        Holiday holiday = await _holidayRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Holiday), request.Id);
        //#region Delete Holiday Date
        //List<DateTime> deleteHolidayDates = new();
        //for (DateOnly date = holiday.StartDate; date <= holiday.EndDate; date = date.AddDays(1))
        //{
        //    deleteHolidayDates.Add(date.ToDateTime(new TimeOnly(0, 0)));
        //}
        //#endregion



        _holidayRepository.Remove(holiday);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<HolidayResponseDto>(holiday);
    }
}
