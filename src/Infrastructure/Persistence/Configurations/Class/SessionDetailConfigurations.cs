namespace Space.Infrastructure.Persistence.Configurations;

public class SessionDetailConfigurations : IEntityTypeConfiguration<SessionDetail>
{
    public void Configure(EntityTypeBuilder<SessionDetail> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder.Property(e => e.StartTime)
               .HasConversion(new TimeOnlyNullableDbConverter());

        builder.Property(e => e.EndTime)
               .HasConversion(new TimeOnlyNullableDbConverter());

    }
}
