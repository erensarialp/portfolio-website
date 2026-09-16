using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Services
{
    public class ServiceRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1500)]
        public string? Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;
    }
}