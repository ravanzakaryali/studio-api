namespace Space.Infrastructure.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder
            .HasOne(s => s.Student)
            .WithOne(c => c.Contact)
            .HasForeignKey<Student>(c => c.ContactId);
    }
}
