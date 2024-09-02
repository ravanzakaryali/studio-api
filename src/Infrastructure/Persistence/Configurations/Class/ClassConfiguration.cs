namespace Space.Infrastructure.Persistence.Configurations;

internal class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder.Property(c => c.IsNew).HasDefaultValue(false);
        builder
            .HasOne(c => c.Program)
            .WithMany(p => p.Classes)
            .HasForeignKey(c => c.ProgramId);

        builder.Property(c => c.VitrinDate)
            .HasConversion(new DateOnlyDbConverter());

        builder.Property(e => e.EndDate)
             .HasConversion(new DateOnlyDbConverter());

        builder.Property(e => e.StartDate)
             .HasConversion(new DateOnlyDbConverter());
    }
}
