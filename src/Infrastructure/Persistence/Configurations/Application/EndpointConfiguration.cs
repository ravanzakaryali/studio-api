
namespace Space.Infrastructure.Persistence.Configurations;

public class EndpointConfiguration : IEntityTypeConfiguration<E.Endpoint>
{
    public void Configure(EntityTypeBuilder<Endpoint> builder)
    {
        builder.ConfigureBaseEntity();
        builder
            .Property(r => r.HttpMethod)
            .HasDefaultValue(HttpMethodEnum.GET);

    }
}
