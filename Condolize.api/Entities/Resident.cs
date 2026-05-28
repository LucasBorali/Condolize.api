using Condolize.api.Entities.Common;

namespace Condolize.api.Entities
{
    public class Resident : BaseEntity
    {
        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public Guid UnitId { get; set; }

        public Unit Unit { get; set; } = null!;
    }
}
