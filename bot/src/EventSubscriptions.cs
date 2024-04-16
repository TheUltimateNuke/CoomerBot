using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Discord;
using Genbox.Wikipedia;
using MessageExtensions = Discord.MessageExtensions;

namespace CoomerBot;

public static partial class EventSubscriptions 
{
    private static bool RegexMatch(string regex, string toMatch, out string? matchIndex)
    {
        var regObj = new Regex(regex);
        matchIndex = regObj.Match(toMatch).ToString();
        return regObj.IsMatch(toMatch);
    }

    private static bool IsValidMessage(IMessage message)
    {   
        if (message.Author.IsBot) return false;
        if (string.IsNullOrWhiteSpace(message.Content)) return false;

        return true;
    }

    public static async Task Chair(IMessage message)
    {
        if (!IsValidMessage(message)) return;
        if (!RegexMatch(@"\b(chair)\b", message.Content.ToLower(), out _)) return;
        if (message is not IUserMessage castedUserMessage) return;

        await MessageExtensions.ReplyAsync(castedUserMessage, "A chair is a piece of furniture with a raised surface used to sit on, commonly for use by one person. Chairs are most often supported by four legs and have a back; however, a chair can have three legs or could have a different shape. A chair without a back or arm rests is a stool, or when raised up, a bar stool. A chair with arms is an armchair and with folding action and reclining footrest, a recliner. A permanently fixed chair in a train or theater is a seat or, in an airplane, airline seat; when riding, it is a saddle and bicycle saddle, and for an automobile, a car seat or infant car seat. With wheels it is a wheelchair and when hung from above, a swing. A chair for more than one person is a couch, sofa, settee, or \"loveseat\"; or a bench. A separate footrest for a chair is known as an ottoman, hassock or pouffe.");
    }

    public static async Task Wikipedia(IMessage message)
    {
        var regexMatch = @"(what is an? |what'?s an? |what'?s |what is |what are |who is |who was |who are )";

        if (!IsValidMessage(message)) return;
        if (!RegexMatch(regexMatch, message.Content.ToLower(), out var matchIndex)) return;
        if (message is not IUserMessage castedUserMessage) return;

        try 
        {
            var split = message.Content.ToLower().Split(matchIndex);
            var pageString = split[1];

            using WikipediaClient client = new();

            WikiSearchRequest req = new(pageString)
            {
                Limit = 1
            };

            WikiSearchResponse resp = await client.SearchAsync(req);
            if (resp.QueryResult == null) throw new Exception("QueryResult is null!");
            if (resp.QueryResult.SearchResults.Count == 0) throw new Exception($"No results matching the requested search '{pageString}' was found.");

            var result = resp.QueryResult.SearchResults[0];
            
            using HttpClient webClient = new();
            var apiResponse = await webClient.GetStringAsync($"https://en.wikipedia.org/w/api.php?action=query&exintro=&explaintext=&format=json&pageids={result.PageId}&prop=extracts&redirects=1");
            var summary = JsonNode.Parse(apiResponse).AsObject()["query"]["pages"][result.PageId.ToString()]["extract"].AsValue().ToString();
            
            if (summary.Length > 2000) 
            {
                for (int i = 0; i < summary.Length; i += 2000) 
                {
                    var piece = summary[i..(summary.Length < i + 2000 ? summary.Length : i + 2000)];
                    await MessageExtensions.ReplyAsync(castedUserMessage, piece);
                }
            }
            else
            {
                await MessageExtensions.ReplyAsync(castedUserMessage, summary);
            }
        }
        catch (Exception error)
        {
            await MessageExtensions.ReplyAsync(castedUserMessage, $"There's nothing there.\n\n||This message is being displayed because an error occurred searching Wikipedia: {error}||");
        }
    }
}