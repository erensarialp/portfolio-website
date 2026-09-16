using System.ComponentModel.DataAnnotations;
using Portfolio.Domain.Enums;

namespace Portfolio.Api.Models.Portfolio
{
    public class UpdatePortfolioItemRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required]
        public PortfolioCategory Category { get; set; }

        [MaxLength(200)]
        public string? Client { get; set; }

        public int? Year { get; set; }

        [MaxLength(200)]
        public string? Format { get; set; }

        [MaxLength(1000)]
        public string? Result { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }
}