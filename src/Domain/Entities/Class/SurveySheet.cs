using System;
namespace Space.Domain.Entities
{
	public class SurveySheet:BaseAuditableEntity
	{
        public Module Module { get; set; }
        public int ModuleId { get; set; }

        public Class Class { get; set; }
        public int ClassId { get; set; }
    }
}

