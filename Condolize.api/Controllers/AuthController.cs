using Condolize.api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Condolize.api.Services;
using Condolize.api.DTOs;
using Condolize.api.Services;

namespace Condolize.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, PasswordService passwordService, TokenService tokenService)
        {
            _context = context;
            _passwordService = passwordService;
            _tokenService = tokenService;
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

            return Ok(new { token});


        }
        


    }
}
