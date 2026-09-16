using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Models.Portfolio;
using Portfolio.Api.Models.PortfolioMedia;
using Portfolio.Domain.Enums;
using Portfolio.Infrastructure.Data;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/portfolio")]
    public class PortfolioController : ControllerBase
    {
        private readonly PortfolioDbContext _context;

        public PortfolioController(PortfolioDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
     [FromQuery] PortfolioCategory? category)
        {
            var query = _context.PortfolioItems
                .AsNoTracking()
                .Where(x => x.IsActive)
                .AsQueryable();

            if (category.HasValue)
            {
                query = query.Where(x => x.Category == category.Value);
            }

            var items = await query
                .OrderBy(x => x.DisplayOrder)
                .ThenByDescending(x => x.CreatedAt)
                .Select(x => new PortfolioItemResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    Description = x.Description,
                    Category = x.Category,
                    Client = x.Client,
                    Year = x.Year,
                    Format = x.Format,
                    Result = x.Result,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,

                    Media = x.Media
                        .OrderByDescending(m => m.IsCover)
                        .ThenBy(m => m.DisplayOrder)
                        .Select(m => new PortfolioMediaPublicResponse
                        {
                            Id = m.Id,
                            MediaType = m.MediaType,
                            AspectRatio = m.AspectRatio,
                            StorageKey = m.StorageKey,
                            ThumbnailStorageKey = m.ThumbnailStorageKey,
                            TeaserStorageKey = m.TeaserStorageKey,
                            AltText = m.AltText,
                            IsCover = m.IsCover,
                            DisplayOrder = m.DisplayOrder
                        })
                        .ToList()
                })
                .ToListAsync();

            return Ok(items);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var item = await _context.PortfolioItems
                .AsNoTracking()
                .Where(x => x.Slug == slug && x.IsActive)
                .Select(x => new PortfolioItemResponse
                {
                    Id = x.Id,
                    Title = x.Title,
                    Slug = x.Slug,
                    Description = x.Description,
                    Category = x.Category,
                    Client = x.Client,
                    Year = x.Year,
                    Format = x.Format,
                    Result = x.Result,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,

                    Media = x.Media
                        .OrderByDescending(m => m.IsCover)
                        .ThenBy(m => m.DisplayOrder)
                        .Select(m => new PortfolioMediaPublicResponse
                        {
                            Id = m.Id,
                            MediaType = m.MediaType,
                            AspectRatio = m.AspectRatio,
                            StorageKey = m.StorageKey,
                            ThumbnailStorageKey = m.ThumbnailStorageKey,
                            TeaserStorageKey = m.TeaserStorageKey,
                            AltText = m.AltText,
                            IsCover = m.IsCover,
                            DisplayOrder = m.DisplayOrder
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (item is null)
            {
                return NotFound(new
                {
                    message = "Portfolyo projesi bulunamadı."
                });
            }

            return Ok(item);
        }
    }
}