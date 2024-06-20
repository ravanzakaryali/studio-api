using Microsoft.EntityFrameworkCore.ChangeTracking;
using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record CreateHolidayCommand(string Description, DateOnly StartDate, DateOnly EndDate) : IRequest<HolidayResponseDto>;

internal class CreateHolidayCommandHandler : IRequestHandler<CreateHolidayCommand, HolidayResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;

    public CreateHolidayCommandHandler(
        ISpaceDbContext spaceDbContext)
    {
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
                Description = request.Description,
            }, cancellationToken);

        #region Create Holiday Date
        List<DateOnly> holidayDates = new();
        for (DateOnly date = holidayEntry.Entity.StartDate; date <= holidayEntry.Entity.EndDate; date = date.AddDays(1))
        {
            holidayDates.Add(date);
        }
        #endregion

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
