using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CoomerBot;

public class Program
{
    public const string DOWNLOAD_LINK = "https://api.github.com/repos/TheUltimateNuke/CoomerBot/releases/latest";
    public const string LOCAL_VERSION = "1.2.0";

    private const float updateCheckMins = 1f;

    public static DiscordSocketClient? DiscordClient {get; private set;}
    public static InteractionService? Service {get; private set;}
    
    public static readonly HttpClient webClient = new();
    public static readonly Version localVersion = new(LOCAL_VERSION);

    private static readonly PeriodicTimer updateCheckTimer = new(TimeSpan.FromMinutes(updateCheckMins));
    private static bool alreadySubscribed = false;

    private static readonly DiscordSocketConfig config = new() 
    { 
        MessageCacheSize = 100,
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
    };

    private static async Task Main()
    {
        DiscordClient = new(config);
        Service = new(DiscordClient.Rest);
        
        DiscordClient.Log += Log;

        var token = Environment.GetEnvironmentVariable("COOMERBOT_TOKEN");
        if (token is null)
        {
            Console.WriteLine("COOMERBOT_TOKEN is null! The program will not execute.");
            return;
        }

        await DiscordClient.LoginAsync(TokenType.Bot, token);
        await DiscordClient.StartAsync();
        
        DiscordClient.Ready += () => 
        {
            if (alreadySubscribed) return Task.CompletedTask;

            SubscribeToEventsOnce();

            alreadySubscribed = true;
            return Task.CompletedTask;
        };

        while (await updateCheckTimer.WaitForNextTickAsync())
        {
            Console.WriteLine("Checking for updates. . .");
            
            
            
            Environment.Exit(0);
        }

        await Task.Delay(-1);
    }

    private static void UpdateSelf() 
    {
        
    }

    private static Task SubscribeToEventsOnce() 
    {
        if (DiscordClient is null) return Task.CompletedTask;

        var thisAssembly = Assembly.GetExecutingAssembly();
        foreach (Type assemblyType in thisAssembly.GetTypes()) 
        {
            foreach (MethodInfo method in assemblyType.GetMethods()) 
            {
                var methodAttribute = method.GetCustomAttribute(typeof(EventSubAttribute));
                if (methodAttribute is not EventSubAttribute castedAttr) continue;

                try 
                {
                    switch (castedAttr.eventType) 
                    {
                        case EventSubAttribute.SupportedEventType.MESSAGE_RECEIVED:
                            DiscordClient.MessageReceived += method.CreateDelegate<Func<IMessage, Task>>();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error occured subscribing to event with {method.DeclaringType}.{method.Name}: {e}");
                }
            }
        }

        return Task.CompletedTask;
    }

    private static Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}