﻿namespace Space.Infrastructure.Persistence.Configurations;

public class AttendanceWorkerConfiguration : IEntityTypeConfiguration<AttendanceWorker>
{
    public void Configure(EntityTypeBuilder<AttendanceWorker> builder)
    {
        builder.ConfigureBaseAuditableEntity();

        builder
            .Property(r => r.AttendanceStatus)
            .HasConversion(new EnumToStringConverter<AttendanceStatus>());
    }
}
