namespace Space.Infrastructure.Helpers;

public class DateOnlyDbConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyDbConverter() : base(
        d => d.ToDateTime(TimeOnly.MinValue), 
        dt => DateOnly.FromDateTime(dt)) 
    { }
}
