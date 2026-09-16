namespace Portfolio.Api.Services.Chat
{
    public interface IChatSessionStore
    {
        ChatSession GetOrCreate(Guid? sessionId);
    }
}