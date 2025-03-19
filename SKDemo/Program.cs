using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SKDemo;

var builder = Kernel.CreateBuilder();

// Services
builder.AddAzureOpenAIChatCompletion("gpt-4o-mini",
    "https://yooho-m8fynziw-eastus2.openai.azure.com/openai/deployments/gpt-4o-mini/chat/completions?api-version=2025-01-01-preview",
    "CPZQx8rwqTmNk2c91rSyl8r68kPB9K70bXU4GcVaI1kkksn5ya9VJQQJ99BCACHYHv6XJ3w3AAAAACOGwXSo");

// Plugins
builder.Plugins.AddFromType<NewsPlugin>();

var kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();
var chatMessages = new ChatHistory();

while (true)
{
    Console.Write("Prompt:");
    chatMessages.AddUserMessage(Console.ReadLine());

    var completion = chatService.GetStreamingChatMessageContentsAsync(
        chatMessages,
        executionSettings: new OpenAIPromptExecutionSettings()
        {
            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
        },
        kernel: kernel);

    var fullMessage = "";
    await foreach (var content in completion)
    {
        Console.WriteLine(content.Content);
        fullMessage += content.Content;
    }

    chatMessages.AddAssistantMessage(fullMessage);
    Console.WriteLine();
}


Console.WriteLine("Hello, World!");
