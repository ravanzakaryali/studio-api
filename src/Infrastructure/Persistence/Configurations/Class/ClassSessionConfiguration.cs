namespace Space.Infrastructure.Persistence.Configurations;

public class ClassSessionConfiguration : IEntityTypeConfiguration<ClassTimeSheet>
{
    public void Configure(EntityTypeBuilder<ClassTimeSheet> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder
            .Property(r => r.Category)
            .HasConversion(new EnumToStringConverter<ClassSessionCategory>());


        builder.Property(e => e.StartTime)
               .HasConversion(new TimeOnlyDbConverter());

        builder.Property(e => e.EndTime)
               .HasConversion(new TimeOnlyDbConverter());
    }
}
