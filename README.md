# CoomerBot

> Hello, Gordon!  
> \- Dr. Coomer, [Half-Life VR but the AI is Self Aware (HLVRAI)](https://www.youtube.com/watch?v=vDUYLDtC5Qw&list=PLglTodSj6fQGbLTtPF_YXVJ6TKSaC3O02)

## Repository Details

This repository contains the source code for [my (see LICENSE)](./LICENSE) implementation of a Dr. Coomer discord bot (token not included, obviously). **The bot spouts various famous quotes upon hearing specific phrases - which are listed below - along with a (WIP) toggleable "Tamagotchi mode"** that forces you to feed him, water him, etc. as a bit of reoccuring maintenance on the ol' Discord server. Admins should be able to set the channel he sends Tamagotchi-related messages in, alongside some other settings.

## How to get it yourself

Currently you'd have to make your own app through the Discord Developer Portal and add the generated bot token into your environment variables as COOMERBOT_TOKEN. From there you can simply run the program as downloaded from the Releases page or compiled from source.  
Eventually I am hoping to release my own trustworthy and secure version of the bot to the public for people who don't want/don't have the knowledge to do that.

## Various Functions

| Command/Message | Response |
|---|---|
| chair | A chair is a piece of furniture with a raised surface used to sit on, commonly for use by one person. Chairs are most often supported by four legs and have a back; however, a chair can have three legs or could have a different shape. A chair without a back or arm rests is a stool, or when raised up, a bar stool. A chair with arms is an armchair and with folding action and reclining footrest, a recliner. A permanently fixed chair in a train or theater is a seat or, in an airplane, airline seat; when riding, it is a saddle and bicycle saddle, and for an automobile, a car seat or infant car seat. With wheels it is a wheelchair and when hung from above, a swing. A chair for more than one person is a couch, sofa, settee, or \"loveseat\"; or a bench. A separate footrest for a chair is known as an ottoman, hassock or pouffe. |
| i thought _____ | You thought wrong, my good bitch! |
| any variation of who/what/whos/whats (a/an) | Searches Wikipedia for an article matching what's after the command and sends the summary of the article if found, otherwise it sends **"There's nothing there."** along with an error code. |
| wife | I had a wife, but they took her in the divorce! |
| wikipedia | The free online encyclopedia that anyone can edit! |

## Footnotes & Credits

- Made almost entirely within [GitHub Codespaces](https://github.com/codespaces) and [Gitpod](https://www.gitpod.io) as I'm always on the move these days
- Big thanks to my friend group for getting me into HLVRAI
- Special thanks to [KaylaXD](https://discord.com/users/616440799785123948) in particular for the idea and continued help with brainstorming
- Thanks to my family for still patiently waiting for me to become a millionaire despite me still making weird shit like this :3

## Dependencies

- Highly dependent on the [Discord.Net project](https://github.com/discord-net/Discord.Net)
  - > This is my first time utilizing Discord.Net, so I may have overlooked some uses of the API, such as InteractionService. I will revise this when I feel like it, but for now, if it ain't broke don't fix it!
- Barely uses but still dependent on [Genbox.Wikipedia](https://github.com/Genbox/Wikipedia)
  - > I could probably omit this dependency in the future in favor of fully using HttpClient.
- .NET 6 SDK and Runtime (if it isn't already bundled in the app itself, which it could be)
