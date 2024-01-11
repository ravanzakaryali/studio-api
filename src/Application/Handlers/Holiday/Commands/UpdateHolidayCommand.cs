namespace Space.Application.Handlers;

public record UpdateHolidayCommand(int Id, UpdateHolidayRequestDto UpdateHoliday) : IRequest<HolidayResponseDto>;

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
        Holiday holiday = await _spaceDbContext.Holidays.FindAsync(request.Id)
           ?? throw new NotFoundException(nameof(Holiday), request.Id);

         #region Update Holiday Date
        List<DateOnly> holidayDates = new();
        for (DateOnly date = holiday.StartDate; date <= holiday.EndDate; date = date.AddDays(1))
        {
            holidayDates.Add(date);
        }
        #endregion


        //all classsessions isHoliday false
        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
             .Where(cs => cs.Date >= holiday.StartDate && cs.Date <= holiday.EndDate)
             .ToListAsync(cancellationToken);

        classSessions.ForEach(cs => cs.IsHoliday = false);

        Holiday updateHoliday = new(){
            Id = holiday.Id,
            StartDate = request.UpdateHoliday.StartDate,
            EndDate = request.UpdateHoliday.EndDate,
            Description = request.UpdateHoliday.Description,
        };


         #region New Holiday Date
        List<DateOnly> newHolidayDates = new();
        for (DateOnly date = updateHoliday.StartDate; date <= updateHoliday.EndDate; date = date.AddDays(1))
        {
            holidayDates.Add(date);
        }
        #endregion

         List<ClassSession> newHolidayClassSessions = await _spaceDbContext.ClassSessions
             .Where(cs => cs.Date >= holiday.StartDate && cs.Date <= holiday.EndDate)
             .ToListAsync(cancellationToken);

        newHolidayClassSessions.ForEach(cs => cs.IsHoliday = true);

        _spaceDbContext.Holidays.Update(updateHoliday);
        await _spaceDbContext.SaveChangesAsync();
        return new HolidayResponseDto(){
            Description = updateHoliday.Description,
            ClassId = updateHoliday.ClassId,
            EndDate = updateHoliday.EndDate,
            StartDate = updateHoliday.StartDate,
            Id = updateHoliday.Id
        };
    }
}
