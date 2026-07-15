using Condolize.api.Auth;
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
    public class ReservationsController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly CurrentUserService _currentUserService;

        public ReservationsController(
            AppDbContext context,
            CurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationDto dto)
        {
            var currentUser = _currentUserService.GetUser();

            var space = await _context.PublicSpaces
     .FirstOrDefaultAsync(x =>
         x.Id == dto.PublicSpaceId &&
         x.AssociationId == currentUser.AssociationId);

            if (space is null)
            {
                return BadRequest("Espaço não encontrado.");
            }

            var user = await _context.Users
    .FirstOrDefaultAsync(x =>
        x.Id == dto.UserId &&
        x.AssociationId == currentUser.AssociationId);

            if (user is null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            if (dto.StartDate >= dto.EndDate)
            {
                return BadRequest(
                    "Data inicial deve ser menor que a final.");
            }

            var conflict = await _context.Reservations
    .AnyAsync(x =>
        x.PublicSpaceId == dto.PublicSpaceId &&
        dto.StartDate < x.EndDate &&
        dto.EndDate > x.StartDate);

            if (conflict)
            {
                return BadRequest(
                    "Já existe uma reserva neste período.");
            }


            var reservation = new Reservation
            {
                PublicSpaceId = dto.PublicSpaceId,
                UserId = dto.UserId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Notes = dto.Notes
            };

            await _context.Reservations.AddAsync(reservation);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currentUser = _currentUserService.GetUser();

            var reservations = await _context.Reservations
     .Where(x =>
         x.User.AssociationId ==
         currentUser.AssociationId)
     .Select(x => new ReservationDto
     {
         Id = x.Id,

         PublicSpaceId = x.PublicSpaceId,
         PublicSpaceName = x.PublicSpace.Name,

         UserId = x.UserId,
         UserName = x.User.Name,

         StartDate = x.StartDate,
         EndDate = x.EndDate,

         Notes = x.Notes
     })
     .ToListAsync();

            return Ok(reservations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentUser = _currentUserService.GetUser();

            var reservation = await _context.Reservations
    .Include(x => x.User)
    .FirstOrDefaultAsync(x =>
        x.Id == id &&
        x.User.AssociationId ==
        currentUser.AssociationId);

            if (reservation is null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();

            return NoContent();

        }


    }
}
