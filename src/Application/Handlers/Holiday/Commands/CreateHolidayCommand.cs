using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record CreateHolidayCommand(string Description, DateOnly StartDate, DateOnly EndDate, Guid? ClassId) : IRequest<HolidayResponseDto>;

internal class CreateHolidayCommandHandler : IRequestHandler<CreateHolidayCommand, HolidayResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHolidayRepository _holidayRepository;
    readonly IClassSessionRepository _classSessionRepository;

    public CreateHolidayCommandHandler(IUnitOfWork unitOfWork, IHolidayRepository holidayRepository, IClassSessionRepository classSessionRepository)
    {
        _unitOfWork = unitOfWork;
        _holidayRepository = holidayRepository;
        _classSessionRepository = classSessionRepository;
    }

    public async Task<HolidayResponseDto> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {
        if (request.EndDate <= request.StartDate)
        {
            throw new DateTimeException("Başlağıc tarix son tarixdən böyük ola bilməz");
        }
        if (await _holidayRepository.GetAsync(h => h.StartDate == request.StartDate && h.EndDate == request.EndDate) != null)
        {
            throw new AlreadyExistsException("Bu bayram artıq əlavə olunub");
        }

        Holiday holiday = await _holidayRepository.AddAsync(new Holiday()
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            ClassId = request.ClassId,
            Description = request.Description,
        });

        #region All Holiday Date
        IEnumerable<Holiday> allHolday = await _holidayRepository.GetAllAsync();
        List<DateTime> allHolidayDates = new();
        foreach (Holiday holidayItem in allHolday)
        {
            for (DateOnly date = holiday.StartDate; date <= holiday.EndDate; date = date.AddDays(1))
            {
                allHolidayDates.Add(date.ToDateTime(new TimeOnly(0, 0)));
            }
        }
        #endregion

        #region Create Holiday Date
        List<DateTime> holidayDates = new();
        for (DateOnly date = holiday.StartDate; date <= holiday.EndDate; date = date.AddDays(1))
        {
            holidayDates.Add(date.ToDateTime(new TimeOnly(0, 0)));
        }
        #endregion

        IEnumerable<ClassSession> classSessions = await _classSessionRepository.GetAllAsync(c => holidayDates.Contains(c.Date) &&
        c.Date >= DateTime.UtcNow);

        var classIds = classSessions
                        .GroupBy(c => c.ClassId)
                        .Select(group => new { ClassId = group.Key, Sessions = group })
                        .ToList();

        IEnumerable<ClassSession> allClassSessions = await _classSessionRepository.GetAllAsync(c =>
                    classIds.Select(cl => cl.ClassId).Contains(c.ClassId));

        foreach (var @class in classIds)
        {
            foreach (var session in @class.Sessions)
            {
                IEnumerable<DayOfWeek> dayOfWeeks = classSessions.Where(s => s.ClassId == @class.ClassId).DistinctBy(c => c.Date.DayOfWeek).Select(c => c.Date.DayOfWeek);
                DateTime maxDate = allClassSessions.Where(s => s.ClassId == @class.ClassId).Max(c => c.Date);
                session.Date = GetAddDate(maxDate, allHolidayDates, holidayDates, dayOfWeeks);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new HolidayResponseDto()
        {
            Description = holiday.Description,
            ClassId = holiday.ClassId,
            EndDate = holiday.EndDate,
            StartDate = request.StartDate,
            Id = holiday.Id
        };
    }
    private DateTime GetAddDate(
       DateTime maxDate,
       IEnumerable<DateTime> allHolidaysDate,
       IEnumerable<DateTime> createHolidayDates,
       IEnumerable<DayOfWeek> dayOfWeeks)
    {
        DateTime addDate = maxDate.AddDays(1);
        if (!allHolidaysDate.Contains(addDate) || !createHolidayDates.Contains(addDate))
        {
            if (dayOfWeeks.Contains(addDate.DayOfWeek))
            {
                return addDate;
            }
        }
        return GetAddDate(addDate, allHolidaysDate, createHolidayDates, dayOfWeeks);
    }
}
