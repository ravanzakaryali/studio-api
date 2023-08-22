namespace Space.Infrastructure.Helpers;

public class TimeOnlyDbConverter : ValueConverter<TimeOnly, TimeSpan>
{
    public TimeOnlyDbConverter() : base(
        t => t.ToTimeSpan(),
        t => TimeOnly.FromTimeSpan(t))
    { }
}

public class TimeOnlyNullableDbConverter : ValueConverter<TimeOnly?, TimeSpan?>
{
    public TimeOnlyNullableDbConverter() : base(
    t => t.HasValue ? t.Value.ToTimeSpan() : null,
    t => t.HasValue ? TimeOnly.FromTimeSpan(t.Value) : null)
    { }
}