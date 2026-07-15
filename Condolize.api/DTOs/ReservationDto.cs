namespace Condolize.api.DTOs
{
    public class ReservationDto
    {
        public Guid Id { get; set; }

        public Guid PublicSpaceId { get; set; }

        public string PublicSpaceName { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public string UserName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
