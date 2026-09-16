namespace Portfolio.Domain.Entities
{
    public class SiteSetting
    {
        public Guid Id { get; set; }

        public string BrandName { get; set; } = string.Empty;

        public string? SeoDescription { get; set; }

        public string? Location { get; set; }

        public string? Email { get; set; }

        public string? HeroEyebrow { get; set; }

        public string HeroTitleLine1 { get; set; } = string.Empty;

        public string? HeroTitleLine2 { get; set; }

        public string? HeroTitleLine3 { get; set; }

        public string? HeroDescription { get; set; }

        public string? AboutTitle { get; set; }

        public string? AboutDescription { get; set; }

        public string? PortfolioTitle { get; set; }

        public string? PortfolioDescription { get; set; }

        public string? ServicesTitle { get; set; }

        public string? ProcessTitle { get; set; }

        public string? ContactTitle { get; set; }

        public string? ContactDescription { get; set; }

        public string? InstagramUrl { get; set; }

        public string? WhatsAppUrl { get; set; }

        public string? VimeoUrl { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}