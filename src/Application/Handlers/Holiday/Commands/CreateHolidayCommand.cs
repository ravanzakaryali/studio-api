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
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) != null)
        {
            throw new AlreadyExistsException("Bu bayram artıq əlavə olunub");
        }

        EntityEntry<Holiday> holidayEntry = await _spaceDbContext.Holidays
            .AddAsync(new Holiday()
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ClassId = request.ClassId,
                Description = request.Description,
            }, cancellationToken);

        #region All Holiday Date
        List<DateOnly> allHolidayDates = await _unitOfWork.HolidayService.GetDatesAsync();
        #endregion

        #region Create Holiday Date
        List<DateOnly> holidayDates = new();
        for (DateOnly date = holidayEntry.Entity.StartDate; date <= holidayEntry.Entity.EndDate; date = date.AddDays(1))
        {
            holidayDates.Add(date);
        }
        #endregion

        List<ClassGenerateSession> classSessions = await _spaceDbContext.ClassGenerateSessions
            .Where(c => holidayDates.Contains(c.Date))
            .ToListAsync();

        List<ClassGenerateSession> allClassSessions = await _spaceDbContext.ClassGenerateSessions
            .Where(c => classSessions
                        .GroupBy(c => c.ClassId)
                        .Select(group => new { ClassId = group.Key, Sessions = group })
                        .ToList().Select(cl => cl.ClassId).Contains(c.ClassId))
            .ToListAsync(cancellationToken: cancellationToken);

        foreach (var @class in classSessions
                        .GroupBy(c => c.ClassId)
                        .Select(group => new { ClassId = group.Key, Sessions = group })
                        .ToList())
        {
            foreach (ClassGenerateSession? session in @class.Sessions)
            {
                IEnumerable<DayOfWeek> dayOfWeeks = classSessions.Where(s => s.ClassId == @class.ClassId).DistinctBy(c => c.Date.DayOfWeek).Select(c => c.Date.DayOfWeek);
                DateOnly maxDate = allClassSessions.Where(s => s.ClassId == @class.ClassId).Max(c => c.Date);
                session.Date = GetAddDate(maxDate, allHolidayDates, holidayDates, dayOfWeeks);
            }
        }

        await _spaceDbContext.SaveChangesAsync(cancellationToken);

        return new HolidayResponseDto()
        {
            Description = holidayEntry.Entity.Description,
            ClassId = holidayEntry.Entity.ClassId,
            EndDate = holidayEntry.Entity.EndDate,
            StartDate = request.StartDate,
            Id = holidayEntry.Entity.Id
        };
    }
    //Todo: Service method
    private DateOnly GetAddDate(
       DateOnly maxDate,
       IEnumerable<DateOnly> allHolidaysDate,
       IEnumerable<DateOnly> createHolidayDates,
       IEnumerable<DayOfWeek> dayOfWeeks)
    {
        DateOnly addDate = maxDate.AddDays(1);
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
