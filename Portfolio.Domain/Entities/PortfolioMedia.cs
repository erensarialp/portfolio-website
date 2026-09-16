using Portfolio.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Portfolio.Domain.Entities
{
    public class PortfolioMedia
    {
        public Guid Id { get; set; }

        public Guid PortfolioItemId { get; set; }

        public MediaType MediaType { get; set; }

        public AspectRatio AspectRatio { get; set; }

        public string StorageKey { get; set; } = string.Empty;

        public string? ThumbnailStorageKey { get; set; }

        public string? TeaserStorageKey { get; set; }

        public string? AltText { get; set; }

        public bool IsCover { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        public PortfolioItem PortfolioItem { get; set; } = null!;
    }
}
