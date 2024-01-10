namespace Space.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.HasQueryFilter(b => !b.IsDeleted && b.IsActive);
        builder.HasMany(e => e.UserRoles)
               .WithOne(e => e.User)
               .HasForeignKey(ur => ur.UserId)
               .IsRequired();

    }
}
