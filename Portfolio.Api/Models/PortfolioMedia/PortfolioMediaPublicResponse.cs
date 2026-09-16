using Portfolio.Domain.Enums;

namespace Portfolio.Api.Models.PortfolioMedia
{
    public class PortfolioMediaPublicResponse
    {
        public Guid Id { get; set; }

        public MediaType MediaType { get; set; }

        public AspectRatio AspectRatio { get; set; }

        public string StorageKey { get; set; } = string.Empty;

        public string? ThumbnailStorageKey { get; set; }

        public string? TeaserStorageKey { get; set; }

        public string? AltText { get; set; }

        public bool IsCover { get; set; }

        public int DisplayOrder { get; set; }
    }
}