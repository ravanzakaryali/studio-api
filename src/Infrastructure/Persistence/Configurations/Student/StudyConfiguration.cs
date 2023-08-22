namespace Space.Infrastructure.Configurations;

public class StudyConfiguration : IEntityTypeConfiguration<Study>
{
    public void Configure(EntityTypeBuilder<Study> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder
            .Property(r => r.StudyType)
            .HasConversion(new EnumToStringConverter<StudyType>());
    }
}
