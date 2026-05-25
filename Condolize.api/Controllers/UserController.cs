using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Condolize.api.Data;
using Condolize.api.Services;
using Condolize.api.Entities;
using Condolize.api.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace Condolize.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;

        public UserController(AppDbContext context, PasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            var passwordHash = _passwordService.HashPassword(dto.Password);

           var user = new User
           {
               Name = dto.Name,
               Email = dto.Email,
               PasswordHash = passwordHash,
               Role = dto.Role,
               AssociationId = dto.AssociationId
           };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
