namespace Condolize.api.DTOs
{
    public class UpdatePublicSpaceDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public List<string> Amenities { get; set; } = [];

        public bool IsActive { get; set; }
    }
}
