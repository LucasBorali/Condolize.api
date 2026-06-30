using Condolize.api.Enum;

namespace Condolize.api.DTOs
{
    public class UpdateUserDto
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }
    }
}
