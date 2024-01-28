# Dotnet-gpt
This is a .NET 8 web api that shows the integration with OpenAI ChatGPT. 🤖

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- OpenAI Account & API key


## Create an OpenAI API key

* Go to [OpenAI Platform](https://platform.openai.com/) and log in

* Go to 'API keys'

* Select 'Create new secret key'

## Create a .NET 8 Web Api project

* Install OpenAI package
```
dotnet add package OpenAI
```

* Add your OpenAI API key to ````appsettings.json````
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "OpenAIChatGpt": {
    "Key": "YOUR-API-KEY-HERE"
  }
}
```

* Create a folder called ````Settings````

* Create a static class called ````OpenAIChatGptSettings```` inside of it

* Create a method to obtain the OpenAI key from ````appsettings.json````

```cs
    public static class OpenAIChatGptSettings
    {
        public static WebApplicationBuilder AddOpenAIChatGPT(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var chatGptKey = configuration["OpenAIChatGpt:Key"];

            var chat = new OpenAIAPI(chatGptKey);

            builder.Services.AddSingleton(chat);

            return builder;
        }
    }
```

* Add this settings to ````Program.cs````
```cs
builder.AddOpenAIChatGPT(builder.Configuration);
```

* Create a new controller called ````ChatGptController````

* Create a ````GET```` method to consult Chat GPT based on text
```cs
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
```
On the completions, which are the chat conversations, we're just getting the first text response.

![WebT](/docs/1.png)

![Web API ChatGPT](/docs/2.png)<br />

We used the default model to get the response, but more models can be explored. Some of them are exclusive for paid plans.<br />
[+ More about OpenAI models](https://platform.openai.com/docs/models/overview)

### Troubleshooting
If you got the following error return *'OpenAI API error 429: "You exceeded your current quota, please check your plan and billing details"'* you can troubleshoot:
- If your OpenAI API key was created recently, try to wait a little bit, at least 10 min
- Add a credit or debit card in payment methods in your OpenAI account (if you do, do not forget to set limits to not incur charges)
- Generate a new API key and add it to the project again
