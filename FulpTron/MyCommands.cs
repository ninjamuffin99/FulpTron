using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace FulpTron
{
    public class MyCommands
    {
        [Command("Hi")]
        public async Task Hi(CommandContext ctx)
        {
            await ctx.RespondAsync($"👋 Hi, {ctx.User.Mention}! u stinky lol");
        }

        [Command("Roll")]
        public async Task Roll20(CommandContext ctx, int min = 0, int max = 20)
        {
            var rnd = new Random();
            await ctx.RespondAsync($"🎲 You rolled a: {rnd.Next(min, max)}");
        }

    }
}
