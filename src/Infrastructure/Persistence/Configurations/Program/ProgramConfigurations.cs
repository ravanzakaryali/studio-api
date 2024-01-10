
namespace Space.Infrastructure.Persistence.Configurations;

internal class ProgramConfigurations : IEntityTypeConfiguration<Program>
{
    public void Configure(EntityTypeBuilder<Program> builder)
    {
        builder.ConfigureBaseAuditableEntity();
    }
}
