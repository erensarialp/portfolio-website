using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Models.Services;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Data;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServicesController : ControllerBase
    {
        private readonly PortfolioDbContext _context;

        public ServicesController(PortfolioDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var services = await _context.Services
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new ServiceResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return Ok(services);
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            var services = await _context.Services
                .AsNoTracking()
                .OrderBy(x => x.DisplayOrder)
                .Select(x => new ServiceResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            return Ok(services);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            var service = new Service
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                DisplayOrder = request.DisplayOrder,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Services.Add(service);

            await _context.SaveChangesAsync();

            return Ok(new ServiceResponse
            {
                Id = service.Id,
                Title = service.Title,
                Description = service.Description,
                DisplayOrder = service.DisplayOrder,
                IsActive = service.IsActive,
                CreatedAt = service.CreatedAt,
                UpdatedAt = service.UpdatedAt
            });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            Guid id,
            ServiceRequest request)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(x => x.Id == id);

            if (service is null)
            {
                return NotFound();
            }

            service.Title = request.Title.Trim();
            service.Description = request.Description?.Trim();
            service.DisplayOrder = request.DisplayOrder;
            service.IsActive = request.IsActive;
            service.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var service = await _context.Services
                .FirstOrDefaultAsync(x => x.Id == id);

            if (service is null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}