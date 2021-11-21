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
                GameManager.Users.Add(new User { ID = ctx.Message.Author.Id, UserName = ctx.Message.Author.Username, Gold = GameManager.StartingGold, TimesCollected = 1, LastGoldCollectionTime = DateTime.Now });
                await ctx.RespondAsync($"Welcome {ctx.Message.Author.Username}, your game profile has been created!");
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
            var u = GameManager.GetUserById(ctx.Message.Author.Id);
            await ctx.RespondAsync($"Gold: {u.Gold}    Times Collected: {u.TimesCollected} \nLast Collection TIme: {u.LastGoldCollectionTime} -- Next Collection Time: {u.LastGoldCollectionTime.AddSeconds(GameManager.CollectionCooldown)}");
        }
        [Command("allprofiles")]
        public async Task AllProfiles(CommandContext ctx)
        {
            if (ctx.Message.Author.Id == 257448897746698241)
            {
                foreach (var u in GameManager.Users)
                {
                    await ctx.RespondAsync($"Profile Name: {u.UserName} \nGold: {u.Gold}    Times Collected: {u.TimesCollected} \nLast Collection TIme: {u.LastGoldCollectionTime} -- Next Collection Time: {u.LastGoldCollectionTime.AddSeconds(GameManager.CollectionCooldown)}");
                }
            }
            else
            {
                await ctx.RespondAsync("You do not have permission to use this command :(");
            }
        }
        [Command("give")]
        public async Task Give(CommandContext ctx, int amount, DiscordMember mention)
        {
            var sender = GameManager.GetUserById(ctx.Message.Author.Id);
            var receiver = GameManager.GetUserById(mention.Id);

            if (sender.Gold >= amount)
            {
                sender.Gold -= amount;
                receiver.Gold += amount;

                await ctx.RespondAsync($"{sender.UserName} gave {receiver.UserName} {amount} gold");
                await GameManager.Save();
            }
            else
            {
                await ctx.RespondAsync($"Insufficient Funds. {sender.UserName}'s current funds: {sender.Gold}");
            }
        }
    }
}