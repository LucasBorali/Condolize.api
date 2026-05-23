using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Condolize.api.Data;
using Condolize.api.Entities;
using Condolize.api.DTOs;

namespace Condolize.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AssociationsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var associations = _context.Associations.ToList();
            return Ok(associations);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAssociationDto dto)
        {
            var association = new Association
            {
                Name = dto.Name
            };
            await _context.Associations.AddAsync(association);

            await _context.SaveChangesAsync();

            return Ok(association);
        }
        

    }
}
