﻿namespace Space.Infrastructure.Persistence.Configurations;

public class HeldModuleConfiguration : IEntityTypeConfiguration<HeldModule>
{
    public void Configure(EntityTypeBuilder<HeldModule> builder)
    {
        builder.ConfigureBaseAuditableEntity();
        builder.Property(c => c.ModuleId).IsRequired(false);
        builder.Property(c => c.ExtraModuleId).IsRequired(false);
    }
}
