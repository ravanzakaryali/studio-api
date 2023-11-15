namespace Space.Application.Abstractions;

public interface IClassRepository : IRepository<Class>
{
    (DateTime StartDate, DateTime EndDate) CalculateStartAndEndDate(Session session, Class @class, List<DateTime> holidayDates);
}
