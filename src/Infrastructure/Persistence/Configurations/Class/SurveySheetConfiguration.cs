using System;
namespace Space.Infrastructure.Persistence
{
    public class SurveySheetConfiguration : IEntityTypeConfiguration<SurveySheet>
    {
        public void Configure(EntityTypeBuilder<SurveySheet> builder)
        {
            builder.ConfigureBaseAuditableEntity();
        }
    }

}

