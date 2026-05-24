using Condolize.api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Condolize.api.Services;
using Condolize.api.DTOs;

namespace Condolize.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;

        public AuthController(AppDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
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

            return Ok("Login bem-sucedido");


        }
        


    }
}
