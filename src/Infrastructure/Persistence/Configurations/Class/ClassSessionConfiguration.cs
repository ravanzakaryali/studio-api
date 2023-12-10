namespace Space.Infrastructure.Persistence.Configurations;

public class ClassSessionConfiguration : IEntityTypeConfiguration<ClassSession>
{
    public void Configure(EntityTypeBuilder<ClassSession> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder
            .Property(r => r.Category)
            .HasConversion(new EnumToStringConverter<ClassSessionCategory>());

        builder
            .Property(r => r.Status)
            .HasConversion(new EnumToStringConverter<ClassSessionStatus>())
            .HasDefaultValue(ClassSessionStatus.Offline);

        builder.Property(e => e.StartTime)
               .HasConversion(new TimeOnlyDbConverter());

        builder.Property(e => e.EndTime)
               .HasConversion(new TimeOnlyDbConverter());

        builder.Property(e => e.Date)
             .HasConversion(new DateOnlyDbConverter());

        builder
                .HasOne(s => s.ClassTimeSheet)
                .WithOne(c => c.ClassSession)
                .HasForeignKey<ClassSession>(c => c.ClassTimeSheetId);
    }}
