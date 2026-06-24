namespace Condolize.api.DTOs
{
    public class ResidentListDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid UnitId { get; set; }
        public string UnitIdentifier { get; set; } = string.Empty;
    }
}
