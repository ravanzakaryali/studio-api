namespace Space.Infrastructure.Persistence.Configurations;

public class SupportCategoryConfiguration : IEntityTypeConfiguration<SupportCategory>
{
    public void Configure(EntityTypeBuilder<SupportCategory> builder)
    {
        builder.ConfigureBaseEntity();
    }
}
