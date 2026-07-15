namespace Condolize.api.DTOs
{
    public class CreateReservationDto
    {
        public Guid PublicSpaceId { get; set; }

        public Guid UserId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}
