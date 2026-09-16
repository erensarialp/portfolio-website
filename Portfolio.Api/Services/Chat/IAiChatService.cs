namespace Portfolio.Api.Services.Chat
{
    public interface IAiChatService
    {
        Task<string> GenerateReplyAsync(
            ChatSession session,
            string userMessage,
            string nextQuestion,
            CancellationToken cancellationToken = default);
    }
}