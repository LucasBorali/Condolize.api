
using Condolize.api.Data;
using Condolize.api.DTOs;
using Condolize.api.Entities;
using Condolize.api.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                Name = currentUser.Name,
                Role = currentUser.Role,
                AssociationId = currentUser.AssociationId
            };

            return Ok(dto);
        }


        [HttpPost("register-association")]
        public async Task<IActionResult> RegisterAssociation(
    RegisterAssociationDto dto)
        {
            var emailExists = await _context.Users
                .AnyAsync(x => x.Email == dto.Email);

            if (emailExists)
                return BadRequest("Email já cadastrado.");

            var association = new Association
            {
                Name = dto.AssociationName
            };

            await _context.Associations.AddAsync(association);

            var passwordHash = _passwordService
                .HashPassword(dto.Password);

            var adminUser = new User
            {
                Name = dto.AdminName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = Enum.UserRole.Admin,
                AssociationId = association.Id
            };

            await _context.Users.AddAsync(adminUser);

            await _context.SaveChangesAsync();

            var token = _tokenService.GenerateToken(adminUser);

            return Ok(new
            {
                token
            });
        }

    }
}
