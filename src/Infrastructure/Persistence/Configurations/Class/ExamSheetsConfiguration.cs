using System;

namespace Space.Infrastructure.Persistence;

public class ExamSheetsConfiguration : IEntityTypeConfiguration<ExamSheet>
{
    public void Configure(EntityTypeBuilder<ExamSheet> builder)
    {
        builder.ConfigureBaseAuditableEntity();
    }
}

