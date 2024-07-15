namespace Space.Infrastructure.Persistence;


public class PermissionLevelConfiguration : IEntityTypeConfiguration<PermissionLevel>
{
    public void Configure(EntityTypeBuilder<PermissionLevel> builder)
    {
        builder.ConfigureBaseAuditableEntity();
    }
}