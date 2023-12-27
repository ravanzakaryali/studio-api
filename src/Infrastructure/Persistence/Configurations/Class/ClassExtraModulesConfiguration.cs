
namespace Space.Infrastructure.Persistence.Configurations;

public class ClassExtraModulesConfiguration : IEntityTypeConfiguration<ClassExtraModulesWorkers>
{
    public void Configure(EntityTypeBuilder<ClassExtraModulesWorkers> builder)
    {
        builder.ConfigureBaseAuditableEntity();

        builder.HasOne(e => e.ExtraModule)
            .WithMany(e => e.ClassExtraModulesWorkers)
            .HasForeignKey(e => e.ExtraModuleId)
            .IsRequired();

        builder.HasOne(e => e.Class)
            .WithMany(e => e.ClassExtraModulesWorkers)
            .HasForeignKey(e => e.ClassId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();



        builder.Property(e => e.EndDate)
             .HasConversion(new DateOnlyDbConverter());

        builder.Property(e => e.StartDate)
             .HasConversion(new DateOnlyDbConverter());
    }
}