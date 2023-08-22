namespace Space.Infrastructure.Persistence;

public class ClassModulesWorkerConfiguration : IEntityTypeConfiguration<ClassModulesWorker>
{
    public void Configure(EntityTypeBuilder<ClassModulesWorker> builder)
    {
        builder.ConfigureBaseAuditableEntity();
    }
}
