namespace Space.Application.Abstractions;

public interface IHolidayRepository : IRepository<Holiday>
{
    Task<List<DateTime>> GetDatesAsync();
}
