namespace Space.Infrastructure.Persistence;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {

        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.HasQueryFilter(b => !b.IsDeleted && b.IsActive);
        builder.HasMany(e => e.UserRoles)
               .WithOne(e => e.Role)
               .HasForeignKey(ur => ur.RoleId)
               .IsRequired();
        //builder.HasMany(r => r.ClassModulesWorkers)
        //    .WithOne(c => c.Role)
        //    .HasForeignKey(c => c.RoleId);
            
    }
}
