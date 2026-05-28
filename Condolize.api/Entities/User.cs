using Condolize.api.Entities.Common;
using Condolize.api.Enum;

namespace Condolize.api.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public Guid AssociationId { get; set; }

        public Association Association { get; set; } = null!;

        public ICollection<Resident> Residents { get; set; } = [];
    }
}
