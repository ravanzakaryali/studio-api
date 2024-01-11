namespace Space.Application.Handlers;

public record DeleteHolidayCommand(int Id) : IRequest<HolidayResponseDto>;

internal class DeleteHolidayCommandHandler : IRequestHandler<DeleteHolidayCommand, HolidayResponseDto>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public DeleteHolidayCommandHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<HolidayResponseDto> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
    {
        Holiday holiday = await _spaceDbContext.Holidays.FindAsync(request.Id)
            ?? throw new NotFoundException(nameof(Holiday), request.Id);

        #region Holiday Date
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

        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<HolidayResponseDto>(holiday);
    }
}
