namespace Space.Infrastructure.Persistence.Configurations;

public class SupportConfiguration : IEntityTypeConfiguration<Support>
{
    public void Configure(EntityTypeBuilder<Support> builder)
    {
        builder.Property(p => p.Status)
             .HasConversion(new EnumToStringConverter<SupportStatus>())
            .HasDefaultValue(SupportStatus.Open)
            .IsRequired();

        builder.ConfigureBaseAuditableEntity();
    }
}
