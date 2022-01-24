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
                await ctx.RespondAsync(GameManager.LootBoxes.Count.ToString());
                foreach (var box in GameManager.LootBoxes)
                {
                    string rewardTypeWeights = string.Join(",", box.RewardTypeWeights.Select(x => x.ToString()).ToArray());
                    string goldRewardValues = string.Join(",", box.GoldRewardValues.Select(x => x.ToString()).ToArray());
                    string goldRewardWeights = string.Join(",", box.GoldRewardWeights.Select(x => x.ToString()).ToArray());
                    string lootboxRewardWeights = string.Join(",", box.LootboxRewardWeights.Select(x => x.ToString()).ToArray());

                    await ctx.RespondAsync($"Cost: {box.Cost}, Num Rewards: {box.NumberOfRewards.ToString()}, RewardTypeWeights: {rewardTypeWeights}, GoldRewardValues: {goldRewardValues}, GoldRewardWeights: {goldRewardWeights}, LootboxRewardWeights: {lootboxRewardWeights}");
                }

                // var user = GameManager.GetUserById(ctx.Message.Author.Id);
                // await ctx.RespondAsync($"Number of lootboxes: {user.LootBoxInventory[0]}");
                // var box = new Lootbox
                // {
                //     Cost = 1,

                // };

                // await ctx.RespondAsync($"{GameManager.LootBoxes[0].Cost}");
                // await ctx.RespondAsync($"{GameManager.LootBoxes[0].NumberOfRewards}");
            }
            else
            {
                await ctx.RespondAsync(Shaco.PermissionMessage);
            }
        }
    }
}