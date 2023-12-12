using System.ComponentModel.DataAnnotations.Schema;

namespace Space.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static EntityTypeBuilder<TEntity> ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
       where TEntity : BaseEntity
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
        builder.Property(e => e.IsActive).HasDefaultValue(true);
        builder.HasQueryFilter(b => !b.IsDeleted && b.IsActive);
        return builder;
    }
    public static EntityTypeBuilder<TEntity> ConfigureBaseAuditableEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseAuditableEntity
    {
        builder.ConfigureBaseEntity();
        builder.Property(e => e.CreatedBy).IsRequired(false);
        builder.Property(e => e.CreatedDate).HasDefaultValueSql("Getutcdate()");
        builder.Property(e => e.LastModifiedBy);
        return builder;
    }
}
