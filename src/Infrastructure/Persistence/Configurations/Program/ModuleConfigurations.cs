namespace Space.Infrastructure.Persistence.Configurations;

internal class ModuleConfigurations : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.ConfigureBaseAuditableEntity();

        builder
            .HasOne(m => m.Program)
            .WithMany(p => p.Modules)
            .HasForeignKey(m => m.ProgramId)
            .IsRequired(false);


        builder
            .Property(m => m.IsSurvey)
            .HasDefaultValue(false);

        builder
            .HasOne(m => m.TopModule)
            .WithMany(tm => tm.SubModules)
            .HasForeignKey(m => m.TopModuleId)
            .IsRequired(false);

    }
}
