using System;
namespace Space.Domain.Entities
{
    public class Reservation : BaseAuditableEntity
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string People { get; set; } = null!;
    }
}

