using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.VoiceNext;

namespace ShacoDiscordBot
{
    public class RandomCommands : BaseCommandModule
    {

        [Command("add")]
        [Description("Adds two numbers")]
        public async Task Add(CommandContext ctx, params int[] numbers)
        {
            int total = 0;
            foreach (var n in numbers)
            {
                total += n;
            }
            await ctx.Channel
                .SendMessageAsync(total.ToString())
                .ConfigureAwait(false);
        }
        [Command("quote")]
        [Description("A random Shaco quote")]
        public async Task Quote(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync(Shaco.RandomVoiceLine().Description).ConfigureAwait(false);
        }
        [Command("hello")]
        [Description("Says hello")]
        public async Task Hello(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Hello " + ctx.Member.Nickname + "...").ConfigureAwait(false);
        }
        [Command("age")]
        [Description("Displays the age of the user's account")]
        public async Task Age(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Account created: " + ctx.Message.Author.CreationTimestamp.DateTime).ConfigureAwait(false);
        }
        [Command("roll")]
        [Description("Random dice roll, default max is 100")]
        public async Task Roll(CommandContext ctx)
        {
            Random rand = new Random();
            int result = rand.Next(1, 101);

            await ctx.Message.RespondAsync($"{ctx.Member.Nickname} rolled a {result}");
        }
        [Command("roll")]
        [Description("Random dice roll")]
        public async Task Roll(CommandContext ctx, string maxRoll)
        {
            int max = 0;
            int result = 0;

            int.TryParse(maxRoll, out max);
            Random rand = new Random();
            if (max >= 1)
            {
                result = rand.Next(1, max + 1);
                await ctx.Message.RespondAsync($"{ctx.Member.Nickname} rolled a {result}");
            }
            else
            {
                await ctx.Message.RespondAsync($"Invalid argument, enter a positive number");
            }
        }
    }
}