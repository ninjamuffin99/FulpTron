using System;
using DSharpPlus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.Net.WebSocket;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.VoiceNext;
using DSharpPlus.VoiceNext.Codec;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;

namespace FulpTron
{
    class Program
    {
        static DiscordClient discord;
        static CommandsNextModule commands;
        static InteractivityModule interactivity;
        static VoiceNextClient voice;

        static void Main(string[] args)
        {
            var prog = new Program();
            prog.RunBotAsync().GetAwaiter().GetResult();

            //MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task RunBotAsync()
        {
            //Reads the config file
            var json = "";
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            //reads value from the config
            //to our client config
            var cfgjson = JsonConvert.DeserializeObject<ConfigJson>(json);
            var cfg = new DiscordConfiguration
            {
                Token = cfgjson.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };

            discord = new DiscordClient(cfg);
            discord.SetWebSocketClient<WebSocket4NetClient>();

            //hooks some events, so we know
            //whats going on
            discord.Ready += Client_Ready;
            discord.GuildAvailable += Client_GuildAvailable;
            discord.ClientErrored += Client_ClientError;

            var ccfg = new CommandsNextConfiguration
            {
                StringPrefix = "fulp",
                CaseSensitive = false,
                EnableDms = true,
                EnableMentionPrefix = true
            };

            commands = discord.UseCommandsNext(ccfg);


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

                if ((e.Message.Content.ToLower().StartsWith("hi") ||
                    e.Message.Content.ToLower().StartsWith("hey") ||
                    e.Message.Content.ToLower().StartsWith("heya") ||
                    e.Message.Content.ToLower().StartsWith("sup") ||
                    e.Message.Content.ToLower().StartsWith("yo") ||
                    e.Message.Content.ToLower().StartsWith("hello")) && (
                    e.Message.Content.ToLower().EndsWith("tom") ||
                    e.Message.Content.ToLower().EndsWith("fulp") ||
                    e.Message.Content.ToLower().EndsWith("tom fulp") ||
                    e.Message.Content.ToLower().EndsWith("fulperino") ||
                    e.Message.Content.ToLower().EndsWith("fulpster") ||
                    e.Message.Content.ToLower().EndsWith("fulpo")))
                {
                    await e.Message.RespondAsync("Hi friend!");
                }

                if (e.Message.Content.ToLower().StartsWith("good bot"))
                {
                    await e.Message.RespondAsync("Yeah, I know");
                }

                if (e.Message.Content.ToLower().StartsWith("shame on you") || e.Message.Content.ToLower().StartsWith(".r34"))
                {
                    if (e.Message.Content.Length > 13 && !e.Message.Content.ToLower().StartsWith(".r34"))
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


                if (e.Message.Content.ToLower().EndsWith("loves lolis") ||
                    e.Message.Content.ToLower().StartsWith("i love lolis") ||
                    e.Message.Content.ToLower().EndsWith("love lolis") ||
                    e.Message.Content.ToLower().EndsWith("like lolis"))
                {
                    await e.Message.RespondAsync("me too!");
                    await e.Message.RespondWithFileAsync("images/tomloveslolis.jpg");
                }

                if (e.Message.Content.ToLower().StartsWith("i hate lolis"))
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

            commands.CommandExecuted += Commands_CommandExecuted;
            commands.CommandErrored += Commands_CommandErrored;


            commands.RegisterCommands<MyCommands>();
            
            var voicecfg = new VoiceNextConfiguration
            {
                VoiceApplication = VoiceApplication.Music
            };

            voice = discord.UseVoiceNext(voicecfg);

            await discord.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task Client_Ready(ReadyEventArgs e)
        {
            //logs the fact that this event occured
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "FulpTron", "Client is ready to precocess events", DateTime.Now);

            //since method is not async, returns a completed task so that no additional work
            //is done
            return Task.CompletedTask;
        }

        private Task Client_GuildAvailable(GuildCreateEventArgs e)
        {
            //pretty much the same shit as Client_Ready()
            e.Client.DebugLogger.LogMessage(LogLevel.Info, "FulpTron", $"Guild available: {e.Guild.Name}", DateTime.Now);

            return Task.CompletedTask;
        }


        private Task Client_ClientError(ClientErrorEventArgs e)
        {
            // let's log the details of the error that just 
            // occured in our client
            e.Client.DebugLogger.LogMessage(LogLevel.Error, "FulpTron", $"Exception occured: {e.Exception.GetType()}: {e.Exception.Message}", DateTime.Now);

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }


        private Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            // let's log the name of the command and user
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Info, "ExampleBot", $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'", DateTime.Now);

            // since this method is not async, let's return
            // a completed task, so that no additional work
            // is done
            return Task.CompletedTask;
        }

        private async Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            // let's log the error details
            e.Context.Client.DebugLogger.LogMessage(LogLevel.Error, "ExampleBot", $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);

            // let's check if the error is a result of lack
            // of required permissions
            if (e.Exception is ChecksFailedException)
            {
                // yes, the user lacks required permissions, 
                // let them know

                var emoji = DiscordEmoji.FromName(e.Context.Client, ":no_entry:");

                // let's wrap the response into an embed
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Access denied",
                    Description = $"{emoji} You do not have the permissions required to execute this command.",
                    Color = new DiscordColor(0xFF0000) // red
                };
                await e.Context.RespondAsync("", embed: embed);
            }
        }
    }

    // this structure will hold data from config.json
    public struct ConfigJson
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
    }

}
