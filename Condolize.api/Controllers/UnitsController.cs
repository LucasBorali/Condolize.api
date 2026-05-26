using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Condolize.api.Data;
using Condolize.api.Services;
using Condolize.api.Entities;
using Condolize.api.DTOs;

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
        public IActionResult Get()
        {
            var currentUser = _currentUserService.GetUser();
            var units = _context.Units.Where(u => u.AssociationId == currentUser.AssociationId).ToList();
            return Ok(units);
        }
    }
}
