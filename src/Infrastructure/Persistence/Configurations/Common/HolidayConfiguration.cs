namespace Space.Infrastructure.Persistence;

public class HolidayConfiguration : IEntityTypeConfiguration<Holiday>
{
    public void Configure(EntityTypeBuilder<Holiday> builder)
    {
        builder.ConfigureBaseEntity();
        builder.Property(e => e.StartDate)
            .HasConversion(new DateOnlyDbConverter());
        builder.Property(e => e.EndDate)
          .HasConversion(new DateOnlyDbConverter());
    }
}
