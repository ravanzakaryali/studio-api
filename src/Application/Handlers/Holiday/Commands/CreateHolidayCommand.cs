using Microsoft.EntityFrameworkCore.ChangeTracking;
using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record CreateHolidayCommand(string Description, DateOnly StartDate, DateOnly EndDate, Guid? ClassId) : IRequest<HolidayResponseDto>;

internal class CreateHolidayCommandHandler : IRequestHandler<CreateHolidayCommand, HolidayResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;

    public CreateHolidayCommandHandler(
        IUnitOfWork unitOfWork,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<HolidayResponseDto> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {
        if (request.EndDate < request.StartDate)
        {
            throw new DateTimeException("Başlağıc tarix son tarixdən böyük ola bilməz");
        }
        if (await _spaceDbContext.Holidays
            .Where(h => h.StartDate == request.StartDate && h.EndDate == request.EndDate)
            .FirstOrDefaultAsync() != null)
        {
            throw new AlreadyExistsException("Bu bayram artıq əlavə olunub");
        }

        EntityEntry<Holiday> holidayEntry = await _spaceDbContext.Holidays.AddAsync(new Holiday()
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            ClassId = request.ClassId,
            Description = request.Description,
        });

        #region All Holiday Date
        List<DateTime> allHolidayDates = await _unitOfWork.HolidayService.GetDatesAsync();
        #endregion

        #region Create Holiday Date
        List<DateTime> holidayDates = new();
        for (DateOnly date = holidayEntry.Entity.StartDate; date <= holidayEntry.Entity.EndDate; date = date.AddDays(1))
        {
            holidayDates.Add(date.ToDateTime(new TimeOnly(0, 0)));
        }
        #endregion

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Where(c => holidayDates.Contains(c.Date))
            .ToListAsync();

        var classIds = classSessions
                        .GroupBy(c => c.ClassId)
                        .Select(group => new { ClassId = group.Key, Sessions = group })
                        .ToList();

        List<ClassSession> allClassSessions = await _spaceDbContext.ClassSessions.Where(c =>
                    classIds.Select(cl => cl.ClassId).Contains(c.ClassId)).ToListAsync();

        foreach (var @class in classIds)
        {
            foreach (var session in @class.Sessions)
            {
                IEnumerable<DayOfWeek> dayOfWeeks = classSessions.Where(s => s.ClassId == @class.ClassId).DistinctBy(c => c.Date.DayOfWeek).Select(c => c.Date.DayOfWeek);
                DateTime maxDate = allClassSessions.Where(s => s.ClassId == @class.ClassId).Max(c => c.Date);
                session.Date = GetAddDate(maxDate, allHolidayDates, holidayDates, dayOfWeeks);
            }
        }

        await _spaceDbContext.SaveChangesAsync();
        return new HolidayResponseDto()
        {
            Description = holidayEntry.Entity.Description,
            ClassId = holidayEntry.Entity.ClassId,
            EndDate = holidayEntry.Entity.EndDate,
            StartDate = request.StartDate,
            Id = holidayEntry.Entity.Id
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
