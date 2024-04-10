using Discord;
using Discord.WebSocket;

namespace CoomerBot;

[AttributeUsage(AttributeTargets.Method)]
public class SubscribeEventAttribute(Program.SupportedEventType type) : Attribute
{
    public readonly Program.SupportedEventType supportedEventType = type;
}