using Condolize.api.Auth;
using Condolize.api.Data;
using Condolize.api.DTOs;
using Condolize.api.Services;
using Condolize.api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace Condolize.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;
        private readonly CurrentUserService _currentUserService;

        public AuthController(AppDbContext context, PasswordService passwordService, TokenService tokenService, CurrentUserService currentUserService)
        {
            _context = context;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return Unauthorized("Usuário ou senha inválidos");
            }

            var passwordValid = _passwordService.VerifyPassword(dto.Password, user.PasswordHash);

            if (!passwordValid)
                return Unauthorized("Usuário ou senha inválidos.");

            var token = _tokenService.GenerateToken(user);

            return Ok(new { token });


        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            var currentUser = _currentUserService.GetUser();

            var email = User.FindFirstValue(ClaimTypes.Email);

            var dto = new MeDto
            {
                UserId = currentUser.UserId,
                Email = email!,
                Role = currentUser.Role,
                AssociationId = currentUser.AssociationId
            };

            return Ok(dto);
        }

    }
}
