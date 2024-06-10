
namespace Space.Infrastructure.Persistence;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder.Property(b => b.AllUsers)
                .HasDefaultValue(false);
    }
}
