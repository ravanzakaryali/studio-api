namespace Space.Infrastructure.Persistence.Configurations;

public class HeldModuleConfiguration : IEntityTypeConfiguration<HeldModule>
{
    public void Configure(EntityTypeBuilder<HeldModule> builder)
    {
        builder.ConfigureBaseAuditableEntity();
    }
}
