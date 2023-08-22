namespace Space.Infrastructure.Persistence.Configurations;

internal class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder.Property(c=>c.IsNew).HasDefaultValue(false);
        builder
            .HasOne(c => c.Program)
            .WithMany(p => p.Classes)
            .HasForeignKey(c => c.ProgramId);
    }
}
