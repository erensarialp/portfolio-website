using Portfolio.Domain.Enums;

namespace Portfolio.Domain.Entities
{
    public class PortfolioItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        public PortfolioCategory Category { get; set; }

        public string? Client { get; set; }

        public int? Year { get; set; }

        public string? Format { get; set; }

        public string? Result { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<PortfolioMedia> Media { get; set; }
            = new List<PortfolioMedia>();
    }
}