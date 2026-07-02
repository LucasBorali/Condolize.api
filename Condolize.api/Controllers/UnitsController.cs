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
    public class UnitsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CurrentUserService _currentUserService;

        public UnitsController(AppDbContext context, CurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUnitDto dto)
        {
            var currentUser = _currentUserService.GetUser();
            var unit = new Unit
            {
                Identifier = dto.Identifier,
                AssociationId = currentUser.AssociationId
            };
            await _context.Units.AddAsync(unit);
            await _context.SaveChangesAsync();
            return Ok(unit);
        }

     
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = _currentUserService.GetUser();

            var units = await _context.Units
                .Where(x => x.AssociationId == currentUser.AssociationId)
                .Select(x => new UnitDto
                {
                    Id = x.Id,
                    Identifier = x.Identifier,
                    ResidentCount = x.Residents.Count(),

                    Residents = x.Residents
                        .Select(r => new ResidentDto
                        {
                            UserId = r.User.Id,
                            Name = r.User.Name,
                            Email = r.User.Email
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(units);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateUnitDto dto)
        {
            var currentUser = _currentUserService.GetUser();
            var unit = await _context.Units
                .FirstOrDefaultAsync(x => x.Id == id && x.AssociationId == currentUser.AssociationId);
            if (unit == null)
                return NotFound("Unidade não encontrada.");

            unit.Identifier = dto.Identifier;
            await _context.SaveChangesAsync();
            return Ok(new UnitDto
            {
                Id = unit.Id,
                Identifier= unit.Identifier,
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUser = _currentUserService.GetUser();
            var unit = await _context.Units
                .FirstOrDefaultAsync(x => x.Id == id && x.AssociationId == currentUser.AssociationId);
            if (unit == null)
                return NotFound("Unidade não encontrada.");
            if(unit.Residents.Any())
                return BadRequest("Não é possível deletar uma unidade que possui moradores.");

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();
            return NoContent();
        }




    }
}
