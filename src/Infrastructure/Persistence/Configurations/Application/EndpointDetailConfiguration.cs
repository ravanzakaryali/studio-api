namespace Space.Infrastructure.Persistence.Configurations;

public class EndpointDetailConfiguration : IEntityTypeConfiguration<E.EndpointDetail>
{
    public void Configure(EntityTypeBuilder<EndpointDetail> builder)
    {
        builder.ToTable("EndpointDetails");
        builder.Property(c => c.ApplicationModuleId).IsRequired(false);
    }
}