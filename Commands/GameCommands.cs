using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Generic;
using DSharpPlus.Entities;
using System;

namespace ShacoDiscordBot
{
    public class GameCommands : BaseCommandModule
    {
        [Command("joingame")]
        public async Task JoinGame(CommandContext ctx)
        {
            var u = GameManager.GetUserById(ctx.Message.Author.Id);
            if (u == null)
            {
                GameManager.Users.Add(new User(ctx.Message.Author.Id, ctx.Message.Author.Username));

                await ctx.RespondAsync($"Welcome {ctx.Message.Author.Username}, your game profile has been created!\n{Shaco.voiceLines[3].Description}");
                await GameManager.Save();
            }
            else
            {
                await ctx.RespondAsync("You already have a game profile!");
            }
        }
        [Command("profile")]
        public async Task Profile(CommandContext ctx)
        {
            var user = GameManager.GetUserById(ctx.Message.Author.Id);

            await ctx.RespondAsync(embed: UserProfile(user));
        }
        [Command("allprofiles")]
        public async Task AllProfiles(CommandContext ctx)
        {
            if (ctx.Message.Author.Id == 257448897746698241)
            {
                foreach (var u in GameManager.Users)
                {
                    await ctx.RespondAsync(UserProfile(u));
                }
            }
            else
            {
                await ctx.RespondAsync("You do not have permission to use this command :(");
            }
        }
        [Command("gift")]
        public async Task Gift(CommandContext ctx, int amount, DiscordMember mention)
        {
            var sender = GameManager.GetUserById(ctx.Message.Author.Id);
            var receiver = GameManager.GetUserById(mention.Id);

            if (sender.ID == receiver.ID)
            {
                await ctx.RespondAsync("You can't gift yourself gold, dumbass...");
                return;
            }
            if (sender.Gold >= amount)
            {
                sender.Gold -= amount;
                sender.GoldGifted += amount;

                receiver.Gold += amount;
                receiver.GoldReceived += amount;

                await ctx.RespondAsync($"{sender.UserName} gifted {receiver.UserName} {amount} gold");
                await GameManager.Save();
            }
            else
            {
                await ctx.RespondAsync($"Insufficient Funds. {sender.UserName}'s current funds: {sender.Gold}");
            }
        }
        [Command("test")]
        public async Task Test(CommandContext ctx)
        {

            var embed = new DiscordEmbedBuilder
            {
                Title = "Test Embed",
                Description = ":)"
            };
            await ctx.RespondAsync(embed: embed);
        }
        public DiscordEmbedBuilder UserProfile(User user)
        {
            return new DiscordEmbedBuilder
            {
                Title = $"{user.UserName}'s Profile",
                Description = $"Gold: {user.Gold}\nTimes Collected: {user.TimesCollected}\nGold Gifted: {user.GoldGifted}\nGold Received: {user.GoldReceived}\nLast Collection TIme: {user.LastGoldCollectionTime}\nNext Collection Time: {user.LastGoldCollectionTime.AddSeconds(GameManager.CollectionCooldown)}",
                Color = DiscordColor.Red
            };
        }
    }
}