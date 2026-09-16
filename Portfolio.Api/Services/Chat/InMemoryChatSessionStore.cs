using System.Collections.Concurrent;

namespace Portfolio.Api.Services.Chat
{
    public class InMemoryChatSessionStore : IChatSessionStore
    {
        private readonly ConcurrentDictionary<Guid, ChatSession> _sessions =
            new();

        public ChatSession GetOrCreate(Guid? sessionId)
        {
            if (
                sessionId.HasValue &&
                _sessions.TryGetValue(
                    sessionId.Value,
                    out var existingSession)
            )
            {
                return existingSession;
            }

            var session = new ChatSession
            {
                Id = Guid.NewGuid(),
                Step = 0,
                CreatedAt = DateTime.UtcNow
            };

            _sessions[session.Id] = session;

            return session;
        }
    }
}