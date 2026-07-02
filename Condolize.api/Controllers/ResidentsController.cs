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

            var residentExists = await _context.Residents
    .AnyAsync(x =>
        x.UserId == dto.UserId &&
        x.UnitId == dto.UnitId);

            if (residentExists)
            {
                return BadRequest(
                    "Morador já vinculado a esta unidade.");
            }

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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = _currentUserService.GetUser();
            
            var residents = await _context.Residents
                .Where(x => x.User.AssociationId == currentUser.AssociationId).Select(x => new ResidentListDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    UnitId = x.UnitId,
                    UnitIdentifier = x.Unit.Identifier
                }).ToListAsync();

            return Ok(residents);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUser = _currentUserService.GetUser();
            var resident = await _context.Residents
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.User.AssociationId == currentUser.AssociationId &&
                    x.Id == id);


            if (resident is null)
                return NotFound("Morador não encontrado.");

            _context.Residents.Remove(resident);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
