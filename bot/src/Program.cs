using Discord;
using Discord.WebSocket;

namespace CoomerBot;

public class Program
{
    public static DiscordSocketClient? Client {get; private set;}

    private static readonly DiscordSocketConfig config = new() 
    { 
        MessageCacheSize = 100,
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
    };

    private static async Task Main(string[] args)
    {
        Client = new(config);
        Client.Log += Log;

        var token = Environment.GetEnvironmentVariable("COOMERBOT_TOKEN");
        if (token == null)
        {
            Console.WriteLine("COOMERBOT_TOKEN is null! The program will not execute.");
            return;
        }

        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();
        
        Client.Ready += () => 
        {
            Client.MessageReceived += EventSubscriptions.Chair;
            Client.MessageReceived += EventSubscriptions.Wikipedia;
            Client.MessageReceived += EventSubscriptions.IThought;
            return Task.CompletedTask;
        };
        Client.Disconnected += (Exception e) => 
        {
            Client.MessageReceived -= EventSubscriptions.Chair;
            Client.MessageReceived -= EventSubscriptions.Wikipedia;
            Client.MessageReceived -= EventSubscriptions.IThought;
            return Task.CompletedTask;
        };

        await Task.Delay(-1);
    }

    private static Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}