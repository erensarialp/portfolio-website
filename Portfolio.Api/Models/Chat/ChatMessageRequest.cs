using System.ComponentModel.DataAnnotations;

namespace Portfolio.Api.Models.Chat
{
    public class ChatMessageRequest
    {
        public Guid? SessionId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Message { get; set; } = string.Empty;
    }
}