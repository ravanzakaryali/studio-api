namespace Space.Infrastructure.Persistence.Configurations;

public class SupportCategoryConfiguration : IEntityTypeConfiguration<SupportCategory>
{
    public void Configure(EntityTypeBuilder<SupportCategory> builder)
    {

        builder.Property(p => p.Redirect)
             .HasConversion(new EnumToStringConverter<SupportRedirect>())
            .HasDefaultValue(SupportStatus.Open);

        builder.ConfigureBaseEntity();
    }
}
