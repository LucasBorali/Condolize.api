namespace Condolize.api.DTOs
{
    public class CreatePublicSpaceDto
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Capacity { get; set; }

        public List<string> Amenities { get; set; } = [];
    }
}
