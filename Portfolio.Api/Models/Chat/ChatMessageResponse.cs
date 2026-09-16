namespace Portfolio.Api.Models.Chat
{
    public class ChatMessageResponse
    {
        public Guid SessionId { get; set; }

        public string Message { get; set; } = string.Empty;

        public int Step { get; set; }

        public bool IsComplete { get; set; }

        public string? WhatsAppUrl { get; set; }
    }
}