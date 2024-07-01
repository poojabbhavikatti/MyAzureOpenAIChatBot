using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyChatBot.Models; // Ensure ChatMessage is imported
using MyChatBot.Services;

namespace MyChatBot.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly OpenAIService _openAIService;

        public ChatController(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskQuestion([FromBody] string userMessage)
        {
            if (string.IsNullOrEmpty(userMessage))
            {
                return BadRequest("User message cannot be empty.");
            }

            var response = await _openAIService.GetOpenAIResponse(userMessage);

            return Ok(response);
        }
    }
}
