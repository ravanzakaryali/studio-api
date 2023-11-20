namespace Space.Application.Abstractions.Services;

public interface IHolidayService
{
    Task<List<DateTime>> GetDatesAsync();
}
