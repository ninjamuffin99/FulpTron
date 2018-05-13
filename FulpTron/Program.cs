using System;
using DSharpPlus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.Net.WebSocket;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;

namespace FulpTron
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;
        static InteractivityModule interactivity;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = File.ReadAllText("token.txt"),
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                LogLevel = LogLevel.Debug
            });

            discord.SetWebSocketClient<WebSocket4NetClient>();

            discord.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehaviour = TimeoutBehaviour.Ignore,
                PaginationTimeout = TimeSpan.FromMinutes(5),
                Timeout = TimeSpan.FromMinutes(2)

            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("are we talking about tom fulp"))
                    await e.Message.RespondAsync("I **LOVE** talking about Tom Fulp");

                if ((e.Message.Content.ToLower().StartsWith("hi")||
                    e.Message.Content.ToLower().StartsWith("hey")||
                    e.Message.Content.ToLower().StartsWith("heya")||
                    e.Message.Content.ToLower().StartsWith("sup")||
                    e.Message.Content.ToLower().StartsWith("yo")||
                    e.Message.Content.ToLower().StartsWith("hello"))&&( 
                    e.Message.Content.ToLower().EndsWith("tom")||
                    e.Message.Content.ToLower().EndsWith("fulp")||
                    e.Message.Content.ToLower().EndsWith("tom fulp")||
                    e.Message.Content.ToLower().EndsWith("fulperino")||
                    e.Message.Content.ToLower().EndsWith("fulpster")||
                    e.Message.Content.ToLower().EndsWith("fulpo")))
                {
                    await e.Message.RespondAsync("Hi friend!");
                }

                if (e.Message.Content.ToLower().StartsWith("good bot"))
                {
                    await e.Message.RespondAsync("Yeah, I know");
                }

                if (e.Message.Content.ToLower().StartsWith("shame on you"))
                {
                    if (e.Message.Content.Length > 13)
                    {
                        await e.Message.RespondAsync("**SHAAAAAME on " + new string(e.Message.Content.Skip(13).ToArray()) + "!!!**");
                    }
                    else
                    {
                        await e.Message.RespondAsync("**SHAAAAAME!!!**");
                    }
                }

                if (e.Message.Content.ToLower().StartsWith("lies lies lies"))
                {
                    await e.Message.RespondAsync("yeah!\n\nhttps://www.youtube.com/watch?v=v6cn0mLJVZY");
                }

                if (e.Message.Content.ToLower().EndsWith("loves lolis")|| 
                    e.Message.Content.ToLower().StartsWith("i love lolis")||
                    e.Message.Content.ToLower().EndsWith("love lolis")||
                    e.Message.Content.ToLower().EndsWith("like lolis"))
                {
                    await e.Message.RespondAsync("me too!");
                    await e.Message.RespondWithFileAsync("images/tomloveslolis.jpg");
                }

                if(e.Message.Content.ToLower().StartsWith("i hate lolis"))
                {
                    await e.Message.RespondAsync(";(");
                }

                if (e.Message.Content.ToLower().StartsWith("what's monster mashing?"))
                {
                    await e.Message.RespondAsync("A good game fool, play it!\n\nhttps://www.newgrounds.com/portal/view/707498");
                }

                if (e.Message.Content.ToLower().StartsWith("i like trains"))
                {
                    await e.Message.RespondAsync("vroom\n\nhttps://www.newgrounds.com/portal/view/581989");
                }
            };

            commands = discord.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefix = "fulp",
                CaseSensitive = false
            });

            commands.RegisterCommands<MyCommands>();

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
