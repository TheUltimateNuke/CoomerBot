using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Text.Json.Nodes;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace CoomerBot;

public class Program
{
    public const string DOWNLOAD_LINK = "https://api.github.com/repos/TheUltimateNuke/CoomerBot/releases/latest";
    public const string LOCAL_VERSION = "0.9.0";

    private const float updateCheckMins = 0.1f;

    public static DiscordSocketClient? DiscordClient {get; private set;}
    public static InteractionService? Service {get; private set;}
    public static bool IsDevMode => Environment.GetEnvironmentVariable("DEV") == "true";
    
    public static readonly HttpClient webClient = new();
    public static readonly Version localVersion = new(LOCAL_VERSION);

    private static readonly PeriodicTimer updateCheckTimer = new(TimeSpan.FromMinutes(updateCheckMins));

    private static readonly DiscordSocketConfig config = new() 
    { 
        MessageCacheSize = 100,
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
    };

    private static async Task Main()
    {
        DiscordClient = new(config);
        Service = new(DiscordClient.Rest);

        webClient.DefaultRequestHeaders.Add("User-Agent", "request");
        
        DiscordClient.Log += Log;

        var token = Environment.GetEnvironmentVariable("COOMERBOT_TOKEN");
        var tokenDev = Environment.GetEnvironmentVariable("COOMERBOT_TOKEN_DEV");
        if (token is null && tokenDev is null)
        {
            Console.WriteLine("COOMERBOT_TOKEN(_DEV) is null! The program will not execute.");
            return;
        }

        await DiscordClient.LoginAsync(TokenType.Bot, IsDevMode ? tokenDev : token);
        await DiscordClient.StartAsync();
        
        SubscribeToEventsOnce();
        DiscordClient.Ready += () => 
        {
            Console.WriteLine("CoomerBot is ready!");

            return Task.CompletedTask;
        };

        while (await updateCheckTimer.WaitForNextTickAsync())
        {
            Console.WriteLine("Checking for updates. . .");
            
            var releaseBody = await webClient.GetStringAsync(DOWNLOAD_LINK);
            var releaseObj = JsonNode.Parse(releaseBody);

            if (releaseBody is null || releaseObj is null) continue;
            Console.WriteLine("Found latest release!");

            var releaseTag = releaseObj["tag_name"]?.GetValue<string>();
            if (releaseTag is null) continue;

            if (!Version.TryParse(releaseTag.Replace("v", ""), out var releaseVersion))
            {
                Console.WriteLine($"Release tag could not be parsed to Version: {releaseTag}");
                continue;
            }

            var isPrerelease =  releaseObj["prerelease"]?.AsValue().GetValue<bool>();
            if (releaseVersion > localVersion && IsDevMode || isPrerelease is null || isPrerelease == false)
            {
                Console.WriteLine("Release version > installed version, attempting to update. . .");

                var releaseAssets = releaseObj["assets"]?.AsArray();
                if (releaseAssets is null) continue;

                foreach (var asset in releaseAssets)
                {
                    if (asset is null) continue;

                    var assetObj = asset.AsObject();
                    var assetName = assetObj["name"]?.GetValue<string>();
                    if (assetName is null) continue;

                    var assetUrl = asset["browser_download_url"]?.GetValue<string>();
                    if (assetUrl is null) continue;

                    var assetBytes = await webClient.GetByteArrayAsync(assetUrl);
                    if (assetBytes is null || assetBytes.Length == 0) continue;

                    var binPath = Path.Combine(AppContext.BaseDirectory, "bin");
                    Directory.CreateDirectory(binPath);
                    var outputFile = Path.Combine(binPath, assetName);
                    await File.WriteAllBytesAsync(outputFile, assetBytes);
                    
                    if (Path.GetExtension(outputFile).ToLower() == ".zip")
                    {
                        try 
                        {
                            ZipFile.ExtractToDirectory(outputFile, Directory.CreateDirectory(Path.Combine(binPath, Path.GetFileNameWithoutExtension(outputFile))).ToString());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Failed to extract .zip file downloaded from release: {e}");
                        }
                    }
                }
            }

            Console.WriteLine("Update successful! Running ./scripts/publish/replace_from_bin.sh to replace current installation and restart. . .");
            try 
            {
                var restartStartInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    Arguments = $"{Environment.CurrentDirectory}/scripts/publish/replace_from_bin.sh",
                    RedirectStandardOutput = true
                };

                var process = new Process() 
                {
                    StartInfo = restartStartInfo
                };

                process.Start();
                await process.WaitForExitAsync();

                File.Move(AppContext.BaseDirectory, AppContext.BaseDirectory + ".bak");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Failed to restart: {e}");
            }
        }

        await Task.Delay(-1);
    }

    private static void SubscribeToEventsOnce() 
    {
        if (DiscordClient is null) return;

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
    }

    private static Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}