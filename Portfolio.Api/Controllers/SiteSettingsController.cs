using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Models.SiteSettings;
using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Data;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/site-settings")]
    public class SiteSettingsController : ControllerBase
    {
        private readonly PortfolioDbContext _context;

        public SiteSettingsController(PortfolioDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var setting = await _context.SiteSettings
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (setting is null)
            {
                return NotFound(new
                {
                    message = "Site ayarları henüz oluşturulmamış."
                });
            }

            return Ok(new SiteSettingResponse
            {
                Id = setting.Id,
                BrandName = setting.BrandName,
                SeoDescription = setting.SeoDescription,
                Location = setting.Location,
                Email = setting.Email,
                HeroEyebrow = setting.HeroEyebrow,
                HeroTitleLine1 = setting.HeroTitleLine1,
                HeroTitleLine2 = setting.HeroTitleLine2,
                HeroTitleLine3 = setting.HeroTitleLine3,
                HeroDescription = setting.HeroDescription,
                AboutTitle = setting.AboutTitle,
                AboutDescription = setting.AboutDescription,
                PortfolioTitle = setting.PortfolioTitle,
                PortfolioDescription = setting.PortfolioDescription,
                ServicesTitle = setting.ServicesTitle,
                ContactTitle = setting.ContactTitle,
                ContactDescription = setting.ContactDescription,
                InstagramUrl = setting.InstagramUrl,
                WhatsAppUrl = setting.WhatsAppUrl,
                VimeoUrl = setting.VimeoUrl,
                UpdatedAt = setting.UpdatedAt
            });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            UpdateSiteSettingRequest request)
        {
            var setting = await _context.SiteSettings
                .FirstOrDefaultAsync();

            if (setting is null)
            {
                setting = new SiteSetting
                {
                    Id = Guid.NewGuid()
                };

                _context.SiteSettings.Add(setting);
            }

            setting.BrandName = request.BrandName.Trim();
            setting.SeoDescription = request.SeoDescription?.Trim();
            setting.Location = request.Location?.Trim();
            setting.Email = request.Email?.Trim();
            setting.HeroEyebrow = request.HeroEyebrow?.Trim();
            setting.HeroTitleLine1 = request.HeroTitleLine1.Trim();
            setting.HeroTitleLine2 = request.HeroTitleLine2?.Trim();
            setting.HeroTitleLine3 = request.HeroTitleLine3?.Trim();
            setting.HeroDescription = request.HeroDescription?.Trim();
            setting.AboutTitle = request.AboutTitle?.Trim();
            setting.AboutDescription = request.AboutDescription?.Trim();
            setting.PortfolioTitle = request.PortfolioTitle?.Trim();
            setting.PortfolioDescription = request.PortfolioDescription?.Trim();
            setting.ServicesTitle = request.ServicesTitle?.Trim();
            setting.ContactTitle = request.ContactTitle?.Trim();
            setting.ContactDescription = request.ContactDescription?.Trim();
            setting.InstagramUrl = request.InstagramUrl?.Trim();
            setting.WhatsAppUrl = request.WhatsAppUrl?.Trim();
            setting.VimeoUrl = request.VimeoUrl?.Trim();
            setting.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Site ayarları başarıyla güncellendi."
            });
        }
    }
}