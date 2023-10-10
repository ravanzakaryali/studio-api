using System;
namespace Space.Domain.Entities
{
    public class Reservation : BaseAuditableEntity
    {
        public Reservation()
        {
            Workers = new HashSet<Worker>();
        }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<Worker> Workers { get; set; }
    }
}

