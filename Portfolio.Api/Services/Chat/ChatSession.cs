namespace Portfolio.Api.Services.Chat
{
    public class ChatSession
    {
        public Guid Id { get; set; }

        public int Step { get; set; }

        public string? ServiceType { get; set; }

        public string? ProjectDetails { get; set; }

        public string? Deadline { get; set; }

        public string? Budget { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}