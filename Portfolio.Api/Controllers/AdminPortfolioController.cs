using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Helpers;
using Portfolio.Api.Models.Portfolio;
using Portfolio.Api.Models.PortfolioMedia;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Data;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/admin/portfolio")]
    [Authorize(Roles = "Admin")]
    public class AdminPortfolioController : ControllerBase
    {
        private readonly PortfolioDbContext _context;

        public AdminPortfolioController(PortfolioDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _context.PortfolioItems
                .AsNoTracking()
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _context.PortfolioItems
                .AsNoTracking()
                .Where(x => x.Id == id)
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

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePortfolioItemRequest request)
        {
            var slug = SlugHelper.Generate(request.Title);

            var originalSlug = slug;
            var counter = 1;

            while (await _context.PortfolioItems
                .AnyAsync(x => x.Slug == slug))
            {
                slug = $"{originalSlug}-{counter}";
                counter++;
            }

            var item = new PortfolioItem
            {
                Id = Guid.NewGuid(),
                Title = request.Title.Trim(),
                Slug = slug,
                Description = request.Description?.Trim(),
                Category = request.Category,
                Client = request.Client?.Trim(),
                Year = request.Year,
                Format = request.Format?.Trim(),
                Result = request.Result?.Trim(),
                DisplayOrder = request.DisplayOrder,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.PortfolioItems.Add(item);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = item.Id },
                new PortfolioItemResponse
                {
                    Id = item.Id,
                    Title = item.Title,
                    Slug = item.Slug,
                    Description = item.Description,
                    Category = item.Category,
                    Client = item.Client,
                    Year = item.Year,
                    Format = item.Format,
                    Result = item.Result,
                    DisplayOrder = item.DisplayOrder,
                    IsActive = item.IsActive,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    Media = new List<PortfolioMediaPublicResponse>()
                });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdatePortfolioItemRequest request)
        {
            var item = await _context.PortfolioItems
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item is null)
            {
                return NotFound(new
                {
                    message = "Portfolyo projesi bulunamadı."
                });
            }

            var slug = SlugHelper.Generate(request.Title);

            var originalSlug = slug;
            var counter = 1;

            while (await _context.PortfolioItems
                .AnyAsync(x =>
                    x.Slug == slug &&
                    x.Id != id))
            {
                slug = $"{originalSlug}-{counter}";
                counter++;
            }

            item.Title = request.Title.Trim();
            item.Slug = slug;
            item.Description = request.Description?.Trim();
            item.Category = request.Category;
            item.Client = request.Client?.Trim();
            item.Year = request.Year;
            item.Format = request.Format?.Trim();
            item.Result = request.Result?.Trim();
            item.DisplayOrder = request.DisplayOrder;
            item.IsActive = request.IsActive;
            item.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var media = await _context.PortfolioMedia
                .AsNoTracking()
                .Where(x => x.PortfolioItemId == item.Id)
                .OrderByDescending(x => x.IsCover)
                .ThenBy(x => x.DisplayOrder)
                .Select(x => new PortfolioMediaPublicResponse
                {
                    Id = x.Id,
                    MediaType = x.MediaType,
                    AspectRatio = x.AspectRatio,
                    StorageKey = x.StorageKey,
                    ThumbnailStorageKey = x.ThumbnailStorageKey,
                    TeaserStorageKey = x.TeaserStorageKey,
                    AltText = x.AltText,
                    IsCover = x.IsCover,
                    DisplayOrder = x.DisplayOrder
                })
                .ToListAsync();

            return Ok(new PortfolioItemResponse
            {
                Id = item.Id,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
                Category = item.Category,
                Client = item.Client,
                Year = item.Year,
                Format = item.Format,
                Result = item.Result,
                DisplayOrder = item.DisplayOrder,
                IsActive = item.IsActive,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                Media = media
            });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _context.PortfolioItems
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item is null)
            {
                return NotFound(new
                {
                    message = "Portfolyo projesi bulunamadı."
                });
            }

            _context.PortfolioItems.Remove(item);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}