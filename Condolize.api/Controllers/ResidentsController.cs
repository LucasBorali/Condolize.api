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
    public class ResidentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CurrentUserService _currentUserService;

        public ResidentsController(AppDbContext context, CurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateResidentDto dto)
        {
           var currentUser = _currentUserService.GetUser();

            var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.Id == dto.UserId &&
                x.AssociationId == currentUser.AssociationId);


            if (user is null)
                return BadRequest("Usuário não encontrado.");

            var unit = await _context.Units
           .FirstOrDefaultAsync(x =>
               x.Id == dto.UnitId &&
               x.AssociationId == currentUser.AssociationId);

            if (unit is null)
                return BadRequest("Unidade não encontrada.");

            var resident = new Resident
            {
                UserId = user.Id,
                UnitId = unit.Id
            };

            await _context.Residents.AddAsync(resident);

            await _context.SaveChangesAsync();

            return Ok(resident);
        }
    }
}
