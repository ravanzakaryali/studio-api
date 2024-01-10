namespace Space.Infrastructure.Persistence;

public class ClassModulesWorkerConfiguration : IEntityTypeConfiguration<ClassModulesWorker>
{
  public void Configure(EntityTypeBuilder<ClassModulesWorker> builder)
  {
    builder.ConfigureBaseAuditableEntity();


  
    builder.Property(e => e.EndDate)
      .HasConversion(new DateOnlyDbConverter());

    builder.Property(e => e.StartDate)
         .HasConversion(new DateOnlyDbConverter());
  }
}
