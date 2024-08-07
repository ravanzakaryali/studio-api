
namespace Space.Infrastructure.Persistence;

public class ClassTimeSheetConfiguration : IEntityTypeConfiguration<ClassTimeSheet>
{
    public void Configure(EntityTypeBuilder<ClassTimeSheet> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder
             .Property(r => r.Category)
             .HasConversion(new EnumToStringConverter<ClassSessionCategory>());
        builder
            .Property(r => r.Status)
            .HasConversion(new EnumToStringConverter<ClassSessionStatus>());

        builder.Property(e => e.StartTime)
               .HasConversion(new TimeOnlyNullableDbConverter());

        builder.Property(e => e.EndTime)
               .HasConversion(new TimeOnlyNullableDbConverter());

        builder.HasMany(c => c.Attendances)
            .WithOne(c => c.ClassTimeSheets)
            .HasForeignKey(c => c.ClassTimeSheetsId);

        builder.Property(e => e.Date)
               .HasConversion(new DateOnlyDbConverter());

    }
}
