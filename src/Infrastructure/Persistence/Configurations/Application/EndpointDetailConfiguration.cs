namespace Space.Infrastructure.Persistence.Configurations;

public class EndpointDetailConfiguration : IEntityTypeConfiguration<E.EndpointAccess>
{
    public void Configure(EntityTypeBuilder<EndpointAccess> builder)
    {
        builder.ConfigureBaseEntity();
        builder.Property(c => c.ApplicationModuleId).IsRequired(false);
    }
}