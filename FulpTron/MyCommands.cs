using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using DSharpPlus.Entities;

//namespace FulpTron
//{
    public class MyCommands
    {
        [Command("Hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}!");

            var interactivity = ctx.Client.GetInteractivityModule();
            var msg = await interactivity.WaitForMessageAsync(xm => xm.Author.Id == ctx.User.Id && xm.Content.ToLower() == "how are you?", TimeSpan.FromMinutes(1));
            if (msg != null)
            {
                
                await ctx.RespondAsync($"Im fine, thank you!");
            }   
        }

        [Command("lies lies lies")]
        public async Task lies(CommandContext ctx)
        {
            await ctx.RespondAsync($"https://www.youtube.com/watch?v=v6cn0mLJVZY");
            await ctx.RespondAsync($"yeah!");
        }

        [Command("Shame")]
        public async Task Shame(CommandContext ctx, DiscordUser usr)
        {
            await ctx.RespondAsync($"**SHAME ON {usr.Mention}**");
        }

        [Command("NGFollow")]
        public async Task NGFollow(CommandContext ctx, string usr)
        {
            if (usr == "TomFulp")
            {
                await ctx.RespondAsync($"Go follow our glorious leader {usr} on Newgrounds!\nhttps://{usr}.newgrounds.com");
            }
            else
                await ctx.RespondAsync($"Go follow {usr} on Newgrounds!\nhttps://{usr}.newgrounds.com");
        }

        [Command("Ping")] // let's define this method as a command
        [Description("Example ping command")] // this will be displayed to tell users what this command does when they invoke help
        [Aliases("Pong")] // alternative names for the command
        public async Task Ping(CommandContext ctx) // this command takes no arguments
        {
            // let's trigger a typing indicator to let
            // users know we're working
            await ctx.TriggerTypingAsync();

            // let's make the message a bit more colourful
            var emoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:");

            // respond with ping
            await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms");
        }

        [Command("Roll")]
        public async Task Roll20(CommandContext ctx, int min = 0, int max = 20)
        {
            var rnd = new Random();
            await ctx.RespondAsync($"🎲 You rolled a: {rnd.Next(min, max)}");
        }

        [Command("poll"), Description("Run a poll with reactions.")]
        public async Task Poll(CommandContext ctx, [Description("How long should the poll last.")] TimeSpan duration, [Description("What options should people have.")] params DiscordEmoji[] options)
        {
            // first retrieve the interactivity module from the client
            var interactivity = ctx.Client.GetInteractivityModule();
            var poll_options = options.Select(xe => xe.ToString());

            // then lets present the poll
            var embed = new DiscordEmbedBuilder
            {
                Title = "Poll time!",
                Description = string.Join(" ", poll_options)
            };
            var msg = await ctx.RespondAsync(embed: embed);

            // add the options as reactions
            for (var i = 0; i < options.Length; i++)
                await msg.CreateReactionAsync(options[i]);

            // collect and filter responses
            var poll_result = await interactivity.CollectReactionsAsync(msg, duration);
            var results = poll_result.Reactions.Where(xkvp => options.Contains(xkvp.Key))
                .Select(xkvp => $"{xkvp.Key}: {xkvp.Value}");

            // and finally post the results
            await ctx.RespondAsync(string.Join("\n", results));
        }

        [Command("Beemovie"), Description("Sends a paginated message.")]
        public async Task SendPaginated(CommandContext ctx)
        {
            // first retrieve the interactivity module from the client
            var interactivity = ctx.Client.GetInteractivityModule();

            // generate pages.
            var lipsum = "insert text here lol";
            var lipsum_pages = interactivity.GeneratePagesInEmbeds(lipsum);

            // send the paginator
            await interactivity.SendPaginatedMessage(ctx.Channel, ctx.User, lipsum_pages, TimeSpan.FromMinutes(5), TimeoutBehaviour.Delete);
        }

        
    }

//}
