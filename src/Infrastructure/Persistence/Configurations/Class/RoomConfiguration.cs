namespace Space.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder
            .Property(r => r.Type)
            .HasConversion(new EnumToStringConverter<RoomType>());
    }

}
