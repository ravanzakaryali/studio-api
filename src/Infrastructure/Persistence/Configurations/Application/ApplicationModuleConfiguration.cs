namespace Space.Infrastructure.Persistence.Configurations;

public class ApplicationModuleConfiguration : IEntityTypeConfiguration<ApplicationModule>
{
    public void Configure(EntityTypeBuilder<ApplicationModule> builder)
    {
        builder.ConfigureBaseEntity();
        builder.Property(c => c.Name).IsRequired();
        builder.Property(c => c.Description).IsRequired(false);


        builder.Property(c => c.NormalizedName).IsRequired(false);
        builder.HasIndex(c => c.NormalizedName).IsUnique();

        builder.HasMany(c => c.PermissionGroupPermissionLevelAppModules)
            .WithOne(c => c.ApplicationModule)
            .HasForeignKey(c => c.ApplicationModuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}