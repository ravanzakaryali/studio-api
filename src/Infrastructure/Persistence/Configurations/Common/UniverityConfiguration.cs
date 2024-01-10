namespace Space.Infrastructure.Configurations;

public class UniverityConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder.ConfigureBaseEntity();
    }
}
