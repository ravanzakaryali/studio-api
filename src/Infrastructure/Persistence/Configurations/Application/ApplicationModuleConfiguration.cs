namespace Space.Infrastructure.Persistence.Configurations;

public class ApplicationModuleConfiguration : IEntityTypeConfiguration<ApplicationModule>
{
    public void Configure(EntityTypeBuilder<ApplicationModule> builder)
    {
        builder.ConfigureBaseEntity();
    }
}