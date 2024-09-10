namespace Space.Infrastructure.Persistence;

public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {


        builder.ToTable("Workers");
        builder.Property(c => c.Fincode).HasDefaultValue("Yoxdur").IsUnicode();

        builder
            .Property(r => r.ContractType)
            .HasConversion(new EnumToStringConverter<ContractType>());

    }
}
