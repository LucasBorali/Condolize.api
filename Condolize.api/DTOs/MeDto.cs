namespace Condolize.api.DTOs
{
    public class MeDto
    {
        public Guid UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public Guid AssociationId { get; set; }
    }
}
