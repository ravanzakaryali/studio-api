namespace Space.Application.Abstractions.Services;

public interface IClassService
{
    (DateOnly StartDate, DateOnly EndDate) CalculateStartAndEndDate(Session session, Class @class, List<DateOnly> holidayDates);
}
