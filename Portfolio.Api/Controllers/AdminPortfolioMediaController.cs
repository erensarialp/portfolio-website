using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Models.PortfolioMedia;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Data;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/admin/portfolio-media")]
    [Authorize(Roles = "Admin")]
    public class AdminPortfolioMediaController : ControllerBase
    {
        private readonly PortfolioDbContext _context;

        public AdminPortfolioMediaController(PortfolioDbContext context)
        {
            _context = context;
        }

        [HttpGet("portfolio/{portfolioItemId:guid}")]
        public async Task<IActionResult> GetByPortfolioItemId(Guid portfolioItemId)
        {
            var portfolioExists = await _context.PortfolioItems
                .AsNoTracking()
                .AnyAsync(x => x.Id == portfolioItemId);

            if (!portfolioExists)
            {
                return NotFound(new
                {
                    message = "Portfolyo projesi bulunamadı."
                });
            }

            var media = await _context.PortfolioMedia
                .AsNoTracking()
                .Where(x => x.PortfolioItemId == portfolioItemId)
                .OrderByDescending(x => x.IsCover)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.CreatedAt)
                .Select(x => new PortfolioMediaResponse
                {
                    Id = x.Id,
                    PortfolioItemId = x.PortfolioItemId,
                    MediaType = x.MediaType,
                    AspectRatio = x.AspectRatio,
                    StorageKey = x.StorageKey,
                    ThumbnailStorageKey = x.ThumbnailStorageKey,
                    TeaserStorageKey = x.TeaserStorageKey,
                    AltText = x.AltText,
                    IsCover = x.IsCover,
                    DisplayOrder = x.DisplayOrder,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return Ok(media);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePortfolioMediaRequest request)
        {
            var portfolioExists = await _context.PortfolioItems
                .AnyAsync(x => x.Id == request.PortfolioItemId);

            if (!portfolioExists)
            {
                return BadRequest(new
                {
                    message = "Belirtilen portfolyo projesi bulunamadı."
                });
            }

            if (request.IsCover)
            {
                var currentCovers = await _context.PortfolioMedia
                    .Where(x =>
                        x.PortfolioItemId == request.PortfolioItemId &&
                        x.IsCover)
                    .ToListAsync();

                foreach (var cover in currentCovers)
                {
                    cover.IsCover = false;
                }
            }

            var media = new PortfolioMedia
            {
                Id = Guid.NewGuid(),
                PortfolioItemId = request.PortfolioItemId,
                MediaType = request.MediaType,
                AspectRatio = request.AspectRatio,
                StorageKey = request.StorageKey.Trim(),
                ThumbnailStorageKey =
                    request.ThumbnailStorageKey?.Trim(),
                TeaserStorageKey =
                    request.TeaserStorageKey?.Trim(),
                AltText = request.AltText?.Trim(),
                IsCover = request.IsCover,
                DisplayOrder = request.DisplayOrder,
                CreatedAt = DateTime.UtcNow
            };

            _context.PortfolioMedia.Add(media);

            await _context.SaveChangesAsync();

            return Ok(new PortfolioMediaResponse
            {
                Id = media.Id,
                PortfolioItemId = media.PortfolioItemId,
                MediaType = media.MediaType,
                AspectRatio = media.AspectRatio,
                StorageKey = media.StorageKey,
                ThumbnailStorageKey = media.ThumbnailStorageKey,
                TeaserStorageKey = media.TeaserStorageKey,
                AltText = media.AltText,
                IsCover = media.IsCover,
                DisplayOrder = media.DisplayOrder,
                CreatedAt = media.CreatedAt
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdatePortfolioMediaRequest request)
        {
            var media = await _context.PortfolioMedia
                .FirstOrDefaultAsync(x => x.Id == id);

            if (media is null)
            {
                return NotFound(new
                {
                    message = "Medya kaydı bulunamadı."
                });
            }

            if (request.IsCover)
            {
                var currentCovers = await _context.PortfolioMedia
                    .Where(x =>
                        x.PortfolioItemId == media.PortfolioItemId &&
                        x.Id != id &&
                        x.IsCover)
                    .ToListAsync();

                foreach (var cover in currentCovers)
                {
                    cover.IsCover = false;
                }
            }

            media.MediaType = request.MediaType;
            media.AspectRatio = request.AspectRatio;
            media.StorageKey = request.StorageKey.Trim();
            media.ThumbnailStorageKey =
                request.ThumbnailStorageKey?.Trim();
            media.TeaserStorageKey =
                request.TeaserStorageKey?.Trim();
            media.AltText = request.AltText?.Trim();
            media.IsCover = request.IsCover;
            media.DisplayOrder = request.DisplayOrder;

            await _context.SaveChangesAsync();

            return Ok(new PortfolioMediaResponse
            {
                Id = media.Id,
                PortfolioItemId = media.PortfolioItemId,
                MediaType = media.MediaType,
                AspectRatio = media.AspectRatio,
                StorageKey = media.StorageKey,
                ThumbnailStorageKey = media.ThumbnailStorageKey,
                TeaserStorageKey = media.TeaserStorageKey,
                AltText = media.AltText,
                IsCover = media.IsCover,
                DisplayOrder = media.DisplayOrder,
                CreatedAt = media.CreatedAt
            });
        }

        [HttpPatch("{id:guid}/cover")]
        public async Task<IActionResult> SetCover(Guid id)
        {
            var media = await _context.PortfolioMedia
                .FirstOrDefaultAsync(x => x.Id == id);

            if (media is null)
            {
                return NotFound(new
                {
                    message = "Medya kaydı bulunamadı."
                });
            }

            var portfolioMedia = await _context.PortfolioMedia
                .Where(x =>
                    x.PortfolioItemId == media.PortfolioItemId)
                .ToListAsync();

            foreach (var item in portfolioMedia)
            {
                item.IsCover = item.Id == id;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Kapak medyası başarıyla güncellendi."
            });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var media = await _context.PortfolioMedia
                .FirstOrDefaultAsync(x => x.Id == id);

            if (media is null)
            {
                return NotFound(new
                {
                    message = "Medya kaydı bulunamadı."
                });
            }

            _context.PortfolioMedia.Remove(media);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}