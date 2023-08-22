namespace Space.Infrastructure.Persistence.Configurations;

public class SupportConfiguration : IEntityTypeConfiguration<Support>
{
    public void Configure(EntityTypeBuilder<Support> builder)
    {
        builder.ConfigureBaseAuditableEntity();
    }
}
