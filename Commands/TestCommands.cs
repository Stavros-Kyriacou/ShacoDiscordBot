using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace ShacoDiscordBot
{
    public class TestCommands : VoiceNextCommands
    {
        [Command("test")]
        public async Task Test(CommandContext ctx)
        {
            if (ctx.Message.Author.Id == 257448897746698241)
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Test :)"
                };

                var user = new User(1, "test");

                embed.AddField($"Level: {user.CooldownLevel}", $"CD: {user.CollectionCooldown} Cost: {user.CooldownUpgradeCost}");

                user.CooldownLevel = 20;
                embed.AddField($"Level: {user.CooldownLevel}", $"CD: {user.CollectionCooldown} Cost: {user.CooldownUpgradeCost}");

                user.CooldownLevel = 50;
                embed.AddField($"Level: {user.CooldownLevel}", $"CD: {user.CollectionCooldown} Cost: {user.CooldownUpgradeCost}");

                user.CooldownLevel = 110;
                embed.AddField($"Level: {user.CooldownLevel}", $"CD: {user.CollectionCooldown} Cost: {user.CooldownUpgradeCost}");

                
                await ctx.RespondAsync(embed: embed);
            }
            else
            {
                await ctx.RespondAsync(Shaco.PermissionMessage);
            }
        }
    }
}