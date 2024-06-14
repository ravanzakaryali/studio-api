namespace Space.Infrastructure.Persistence.Configurations;

public class ApplicationModuleConfiguration : IEntityTypeConfiguration<ApplicationModule>
{
    public void Configure(EntityTypeBuilder<ApplicationModule> builder)
    {
        builder.ConfigureBaseEntity();
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Description).IsRequired(false);
    }
}