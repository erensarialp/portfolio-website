using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.SiteSettings
{
    public class UpdateSiteSettingRequest
    {
        [Required]
        [MaxLength(150)]
        public string BrandName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? SeoDescription { get; set; }

        [MaxLength(200)]
        public string? Location { get; set; }

        [EmailAddress]
        [MaxLength(250)]
        public string? Email { get; set; }

        [MaxLength(150)]
        public string? HeroEyebrow { get; set; }

        [Required]
        [MaxLength(200)]
        public string HeroTitleLine1 { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? HeroTitleLine2 { get; set; }

        [MaxLength(200)]
        public string? HeroTitleLine3 { get; set; }

        [MaxLength(1000)]
        public string? HeroDescription { get; set; }

        [MaxLength(200)]
        public string? AboutTitle { get; set; }

        [MaxLength(3000)]
        public string? AboutDescription { get; set; }

        [MaxLength(200)]
        public string? PortfolioTitle { get; set; }

        [MaxLength(1000)]
        public string? PortfolioDescription { get; set; }

        [MaxLength(200)]
        public string? ServicesTitle { get; set; }

        [MaxLength(200)]
        public string? ContactTitle { get; set; }

        [MaxLength(1000)]
        public string? ContactDescription { get; set; }

        [Url]
        [MaxLength(1000)]
        public string? InstagramUrl { get; set; }

        [Url]
        [MaxLength(1000)]
        public string? WhatsAppUrl { get; set; }

        [Url]
        [MaxLength(1000)]
        public string? VimeoUrl { get; set; }
    }
}