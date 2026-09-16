using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Models.Chat;
using Portfolio.Api.Services.Chat;

namespace Portfolio.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(
            IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("message")]
        public async Task<ActionResult<ChatMessageResponse>>
            SendMessage(
                [FromBody] ChatMessageRequest request,
                CancellationToken cancellationToken)
        {
            var response =
                await _chatService.SendMessageAsync(
                    request,
                    cancellationToken);

            return Ok(response);
        }
    }
}