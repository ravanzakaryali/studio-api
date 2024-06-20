namespace Space.Application.Abstractions.Services;

public interface IClassService
{
    (DateOnly StartDate, DateOnly EndDate) CalculateStartAndEndDate(Session session, Class @class, List<DateOnly> holidayDates);
    Task CheckClassAvailabilityAsync(Class @class, DateOnly date);
    Task<DateOnly> EndDateCalculationAsync(Class @class);
}
