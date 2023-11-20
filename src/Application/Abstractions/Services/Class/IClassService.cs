namespace Space.Application.Abstractions.Services;

public interface IClassService
{
    (DateTime StartDate, DateTime EndDate) CalculateStartAndEndDate(Session session, Class @class, List<DateTime> holidayDates);
}
