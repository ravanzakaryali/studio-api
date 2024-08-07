namespace Space.Infrastructure.Persistence.Configurations;

public class AttendanceWorkerConfiguration : IEntityTypeConfiguration<AttendanceWorker>
{
    public void Configure(EntityTypeBuilder<AttendanceWorker> builder)
    {
        builder.ConfigureBaseAuditableEntity();

        builder.Property(e => e.StartTime)
               .HasConversion(new TimeOnlyDbConverter());

        builder.Property(e => e.EndTime)
               .HasConversion(new TimeOnlyNullableDbConverter());
    }
}
