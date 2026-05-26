using Condolize.api.Entities.Common;

namespace Condolize.api.Entities
{
    public class Association : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = [];
        public ICollection<Unit> Units { get; set; } = [];
    }
}
