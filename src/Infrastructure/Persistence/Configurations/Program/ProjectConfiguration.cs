namespace Space.Infrastructure.Persistence.Configurations;

internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder.Property(p=>p.StartDate)
            .HasConversion(new DateOnlyDbConverter());
        builder.Property(p=>p.EndDate)
            .HasConversion(new DateOnlyDbConverter());
    }
}