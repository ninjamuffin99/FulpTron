using System;
using DSharpPlus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Net.WebSocket;

namespace FulpTron
{
    class Program
    {

        static DiscordClient discord;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "MzgxNjA0MjgxOTY4NjIzNjE3.DdWLWg.cbPPqKtmjY1Yp-GaDlu3ZVLmLOM",
                TokenType = TokenType.Bot
            });

            discord.SetWebSocketClient<WebSocket4NetClient>();

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().StartsWith("are we talking about tom fulp"))
                    await e.Message.RespondAsync("I **LOVE** talking about Tom Fulp");
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
