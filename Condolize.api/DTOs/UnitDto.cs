namespace Condolize.api.DTOs
{
    public class UnitDto
    {
        public Guid Id { get; set; }

        public string Identifier { get; set; } = string.Empty;

        public List<ResidentDto> Residents { get; set; } = [];
    }
}
