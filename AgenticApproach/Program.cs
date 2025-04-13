#pragma warning disable SKEXP0110, SKEXP001

using AgenticApproach.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

var kernelBuilder = Kernel.CreateBuilder();

kernelBuilder.AddOpenAIChatCompletion("gpt-4o",
    "API KEY");

kernelBuilder.Plugins.AddFromType<ProductPlugin>("ProductPlugin");

var kernel = kernelBuilder.Build();

var instruction = """
    You are a retail agent. You can answer questions about products and their sales
    """;

var retailAgent = new ChatCompletionAgent()
{
    Name = "RetailAgent",
    Instructions = instruction,
    Kernel = kernel,
    Arguments = new KernelArguments(new PromptExecutionSettings { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() })
};

AgentGroupChat chat = new(retailAgent);

string userInput;
do
{
    Console.WriteLine("Please enter your question");
    userInput = Console.ReadLine()?.Trim()?.ToLower() ?? string.Empty;
    chat.AddChatMessage(new ChatMessageContent(AuthorRole.User, userInput));
    await foreach(var content in chat.InvokeAsync())
    {
        Console.WriteLine("---------------------------------------------------------------------------------");
        Console.WriteLine("Agent response");
        Console.WriteLine("---------------------------------------------------------------------------------");
        Console.WriteLine($"{content.Content}");
        Console.WriteLine("---------------------------------------------------------------------------------");
    }
} while (userInput != "quit");