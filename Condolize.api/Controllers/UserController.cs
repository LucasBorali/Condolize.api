using Condolize.api.Data;
using Condolize.api.DTOs;
using Condolize.api.Entities;
using Condolize.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Condolize.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly CurrentUserService _currentUserService;

        public UserController(AppDbContext context, PasswordService passwordService, CurrentUserService currentUserService )
        {
            _context = context;
            _passwordService = passwordService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {

            var emailExists = await _context.Users
    .AnyAsync(x => x.Email == dto.Email);

            if (emailExists)
                return BadRequest("Email já cadastrado.");

            var passwordHash = _passwordService.HashPassword(dto.Password);
            var currentUser = _currentUserService.GetUser();


            var user = new User
           {
               Name = dto.Name,
               Email = dto.Email,
               PasswordHash = passwordHash,
               Role = dto.Role,
               AssociationId = currentUser.AssociationId
           };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = _currentUserService.GetUser();

            var users = await _context.Users
                .Where(x => x.AssociationId == currentUser.AssociationId)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Role = x.Role.ToString()
                })
                .ToListAsync();

            return Ok(users);
        }
    }
}
