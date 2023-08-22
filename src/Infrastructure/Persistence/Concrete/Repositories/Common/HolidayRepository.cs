namespace Space.Infrastructure.Persistence.Concrete;

internal class HolidayRepository : Repository<Holiday>, IHolidayRepository
{
    public HolidayRepository(SpaceDbContext context) : base(context)
    {
    }
}
