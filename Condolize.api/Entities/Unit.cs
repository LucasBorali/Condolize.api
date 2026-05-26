using Condolize.api.Entities.Common;

namespace Condolize.api.Entities
{
    public class Unit : BaseEntity
    {
        public string Identifier { get; set; } = string.Empty;

        public Guid AssociationId { get; set; }

        public Association Association { get; set; } = null!;
    }
}
