namespace Space.Infrastructure.Persistence.Configurations;

public class ClassSessionConfiguration : IEntityTypeConfiguration<ClassSession>
{
    public void Configure(EntityTypeBuilder<ClassSession> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder
            .Property(r => r.Category)
            .HasConversion(new EnumToStringConverter<ClassSessionCategory>());

        //builder
        //    .Property(r => r.Status)
        //    .HasConversion(new EnumToStringConverter<ClassSessionStatus>());

        builder.Property(e => e.StartTime)
               .HasConversion(new TimeOnlyDbConverter());

        builder.Property(e => e.EndTime)
               .HasConversion(new TimeOnlyDbConverter());
    }
}
