namespace Space.Infrastructure.Persistence;


public class PermissionAccessConfiguration : IEntityTypeConfiguration<PermissionAccess>
{
    public void Configure(EntityTypeBuilder<PermissionAccess> builder)
    {
        builder.ConfigureBaseEntity();
    }
}