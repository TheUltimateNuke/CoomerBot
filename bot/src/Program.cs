using System.Reflection;
using Discord;
using Discord.WebSocket;

namespace CoomerBot;

public class Program
{
    public static DiscordSocketClient? Client {get; private set;}

    private static bool alreadySubscribed = false;

    private static readonly DiscordSocketConfig config = new() 
    { 
        MessageCacheSize = 100,
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
    };

    private static async Task Main()
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
            SubscribeToEventsOnce();
            return Task.CompletedTask;
        };

        await Task.Delay(-1);
    }

    private static Task SubscribeToEventsOnce() 
    {
        if (Client is null) return Task.CompletedTask;
        if (alreadySubscribed) return Task.CompletedTask;

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
                            Client.MessageReceived += method.CreateDelegate<Func<IMessage, Task>>();
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error occured subscribing to event with {method.DeclaringType}.{method.Name}: {e}");
                }
            }
        }
        alreadySubscribed = true;

        return Task.CompletedTask;
    }

    private static Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}