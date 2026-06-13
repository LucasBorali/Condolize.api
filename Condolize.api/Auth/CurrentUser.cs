namespace Condolize.api.Auth
{
    public class CurrentUser
    {
        public Guid UserId { get; set; }

        public Guid AssociationId { get; set; }

        public string Name { get; set; }

        public string Role { get; set; } = string.Empty;


    }
}
