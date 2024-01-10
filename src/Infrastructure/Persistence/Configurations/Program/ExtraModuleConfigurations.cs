namespace Space.Infrastructure.Persistence.Configurations;

public class ExtraModuleConfigurations : IEntityTypeConfiguration<ExtraModule>
{
    public void Configure(EntityTypeBuilder<ExtraModule> builder)
    {
        builder.ConfigureBaseAuditableEntity();

        builder.HasOne(e => e.Program)
            .WithMany(e => e.ExtraModules)
            .HasForeignKey(e => e.ProgramId)
            .IsRequired();

        builder.HasMany(e => e.HeldModules)
            .WithOne(e => e.ExtraModule)
            .HasForeignKey(e => e.ExtraModuleId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}