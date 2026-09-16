using Portfolio.Api.Models.Chat;

namespace Portfolio.Api.Services.Chat
{
    public interface IChatService
    {
        Task<ChatMessageResponse> SendMessageAsync(
            ChatMessageRequest request,
            CancellationToken cancellationToken = default);
    }
}