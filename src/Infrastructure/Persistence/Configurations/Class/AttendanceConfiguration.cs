namespace Space.Infrastructure.Persistence.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder
        .Property(r => r.Status)
        .HasConversion(new EnumToStringConverter<StudentStatus>());
    }
}
