using System.ComponentModel.DataAnnotations;
using Portfolio.Domain.Enums;

namespace Portfolio.Api.Models.PortfolioMedia
{
    public class CreatePortfolioMediaRequest
    {
        [Required]
        public Guid PortfolioItemId { get; set; }

        [Required]
        public MediaType MediaType { get; set; }

        [Required]
        public AspectRatio AspectRatio { get; set; }

        [Required]
        [MaxLength(1000)]
        public string StorageKey { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? ThumbnailStorageKey { get; set; }

        [MaxLength(1000)]
        public string? TeaserStorageKey { get; set; }

        [MaxLength(500)]
        public string? AltText { get; set; }

        public bool IsCover { get; set; }

        public int DisplayOrder { get; set; }
    }
}