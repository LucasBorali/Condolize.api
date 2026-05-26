using Condolize.api.Auth;
using System.Security.Claims;

namespace Condolize.api.Services
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public CurrentUser GetUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user is null)
                throw new Exception("Usuário não autenticado.");

            return new CurrentUser
            {
                UserId = Guid.Parse(
                user.FindFirstValue(ClaimTypes.NameIdentifier)!),

                AssociationId = Guid.Parse(
                user.FindFirstValue("associationId")!),

                Role = user.FindFirstValue(ClaimTypes.Role)!
            }; 

        }
    }
}
