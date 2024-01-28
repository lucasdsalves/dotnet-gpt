using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace dotnet_gpt.api.Controllers
{
    [Route("api/chat-gpt")]
    [ApiController]
    public class ChatGptController : ControllerBase
    {
        private readonly OpenAIAPI _chatGpt;

        public ChatGptController(OpenAIAPI chatGpt)
        {
            _chatGpt = chatGpt;
        }

        [HttpGet]
        [SwaggerOperation("Interact with OpenAI ChatGPT through text")]
        public async Task<IActionResult> GetResponseFromChatGpt(string text)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(text);

            var response = string.Empty;

            var completionRequest = new CompletionRequest
            {
                Prompt = text,
                Model = Model.DefaultModel,
                MaxTokens = 200,
            };

            var result = await _chatGpt.Completions.CreateCompletionAsync(completionRequest);

            if (result.Completions.Any())
                response = result.Completions.First().Text;

            return Ok(response);
        }
    }
}
