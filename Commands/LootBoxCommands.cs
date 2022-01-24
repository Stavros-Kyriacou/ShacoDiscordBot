using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace ShacoDiscordBot
{
    public class LootBoxCommands : VoiceNextCommands
    {
        [Command("inventory")]
        public async Task Inventory(CommandContext ctx)
        {
            var user = GameManager.GetUserById(ctx.Message.Author.Id);
            await ctx.RespondAsync("yo");
        }
    }
}