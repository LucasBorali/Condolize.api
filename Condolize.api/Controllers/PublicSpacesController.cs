using Condolize.api.Data;
using Condolize.api.DTOs;
using Condolize.api.Entities;
using Condolize.api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Condolize.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PublicSpacesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CurrentUserService _currentUserService;

        public PublicSpacesController(
            AppDbContext context,
            CurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = _currentUserService.GetUser();

            var spaces = await _context.PublicSpaces
                .Where(x =>
                    x.AssociationId ==
                    currentUser.AssociationId)
                .Select(x => new PublicSpaceDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Capacity = x.Capacity,
                    Amenities = x.Amenities,
                    IsActive = x.IsActive
                })
                .ToListAsync();

            return Ok(spaces);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePublicSpaceDto dto)
        {
            var currentUser = _currentUserService.GetUser();

            var space = new PublicSpace
            {
                Name = dto.Name,
                Description = dto.Description,
                Capacity = dto.Capacity,
                Amenities = dto.Amenities,
                AssociationId =
                    currentUser.AssociationId
            };

            await _context.PublicSpaces
                .AddAsync(space);

            await _context.SaveChangesAsync();

            return Ok(space);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdatePublicSpaceDto dto)
        {
            var currentUser = _currentUserService.GetUser();

            var space = await _context.PublicSpaces
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.AssociationId ==
                    currentUser.AssociationId);

            if (space == null)
                return NotFound();

            space.Name = dto.Name;
            space.Description = dto.Description;
            space.Capacity = dto.Capacity;
            space.Amenities = dto.Amenities;
            space.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            return Ok(new PublicSpaceDto
            {
                Id = space.Id,
                Name = space.Name,
                Description = space.Description,
                Capacity = space.Capacity,
                Amenities = space.Amenities,
                IsActive = space.IsActive
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUser = _currentUserService.GetUser();

            var space = await _context.PublicSpaces
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.AssociationId ==
                    currentUser.AssociationId);

            if (space == null)
                return NotFound();

            space.IsActive = false;

            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
