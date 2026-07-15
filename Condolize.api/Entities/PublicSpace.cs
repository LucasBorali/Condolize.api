using Condolize.api.Entities.Common;

namespace Condolize.api.Entities
{
    public class PublicSpace : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public List<string> Amenities { get; set; } = [];

        public bool IsActive { get; set; } = true;

        public Guid AssociationId { get; set; }

        public Association Association { get; set; } = null!;

        public ICollection<Reservation> Reservations { get; set; } = [];
    }
}
