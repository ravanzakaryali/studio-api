namespace Space.Application.Handlers;

public record UpdateHolidayCommand(Guid Id, UpdateHolidayRequestDto UpdateHoliday) : IRequest<HolidayResponseDto>;

internal class UpdateHolidayCommandHandler : IRequestHandler<UpdateHolidayCommand, HolidayResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public UpdateHolidayCommandHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<HolidayResponseDto> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
    {
        //Holiday holiday = await _spaceDbContext.Holidays.FindAsync(request.Id)
        //    ?? throw new NotFoundException(nameof(Holiday), request.Id);
        //Holiday updateHoliday = _mapper.Map<Holiday>(request.UpdateHoliday);

        //updateHoliday.Id = holiday.Id;
        //_spaceDbContext.Holidays.Update(updateHoliday);
        //await _spaceDbContext.SaveChangesAsync();
        //return _mapper.Map<HolidayResponseDto>(updateHoliday);
        throw new NotFoundException();
    }
}
