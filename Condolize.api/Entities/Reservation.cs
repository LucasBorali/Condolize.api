using Condolize.api.Entities.Common;

namespace Condolize.api.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid PublicSpaceId { get; set; }

        public PublicSpace PublicSpace { get; set; } = null!;

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
