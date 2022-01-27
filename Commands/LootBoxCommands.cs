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
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{user.UserName}'s Lootbox Inventory",
                Color = DiscordColor.Magenta
            };

            for (int i = 0; i < user.LootBoxInventory.Length; i++)
            {
                embed.AddField($"{i + 1}. Cost: {GameManager.LootBoxes[i].Cost} Gold", user.LootBoxInventory[i].ToString());
            }

            await ctx.RespondAsync(embed: embed);
        }
        [Command("openbox")]
        public async Task OpenBox(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var user = GameManager.GetUserById(ctx.Message.Author.Id);

            var embed = new DiscordEmbedBuilder
            {
                Title = "Select Box to Open",
                Color = DiscordColor.Magenta
            };
            embed.WithFooter("Enter a number between 1-6 to open that type of box");

            for (int i = 0; i < user.LootBoxInventory.Length; i++)
            {
                embed.AddField($"{i + 1}. Lootbox value: {GameManager.LootBoxes[i].Cost} gold", $"Owned: {user.LootBoxInventory[i].ToString()}");
            }

            await ctx.RespondAsync(embed: embed);

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Message.Author);

            if (int.TryParse(message.Result.Content, out int index))
            {
                if (index > 0 && index <= 6)
                {
                    if (user.LootBoxInventory[index - 1] > 0)
                    {
                        await ctx.RespondAsync(embed: LootBoxManager.OpenLootBox(index, user));
                        await GameManager.Save();
                    }
                    else
                    {
                        await ctx.RespondAsync("You dont own any of that type of lootbox, either open a different type or buy some more, bitchass");
                    }
                }
                else
                {
                    await ctx.RespondAsync("Invalid argument");
                }
            }
        }
        [Command("buybox")]
        public async Task BuyBox(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var user = GameManager.GetUserById(ctx.Message.Author.Id);

            var embed = new DiscordEmbedBuilder
            {
                Title = "Select Box to Purchase",
                Color = DiscordColor.Blurple
            };
            embed.WithFooter("Enter a number between 1-6 to buy that type of box");

            for (int i = 0; i < user.LootBoxInventory.Length; i++)
            {
                embed.AddField($"{i + 1}. Lootbox cost: {GameManager.LootBoxes[i].Cost} gold", $"Owned: {user.LootBoxInventory[i].ToString()}");
            }

            await ctx.RespondAsync(embed: embed);

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Message.Author);

            if (int.TryParse(message.Result.Content, out int index))
            {
                if (index > 0 && index <= 6)
                {
                    //check if enough gold
                    if (user.Gold >= GameManager.LootBoxes[index - 1].Cost)
                    {
                        //subtract gold cost
                        user.Gold -= GameManager.LootBoxes[index - 1].Cost;
                        //add to inventory
                        user.LootBoxInventory[index - 1]++;
                        //add user stats
                        user.GoldSpentOnLootBoxes += GameManager.LootBoxes[index - 1].Cost;
                        user.GoldSpent += GameManager.LootBoxes[index - 1].Cost;
                        await ctx.RespondAsync("Lootbox Purchased!");
                        await GameManager.Save();
                    }
                    else
                    {
                        var difference = GameManager.LootBoxes[index - 1].Cost - user.Gold;
                        await ctx.RespondAsync("Not enough funds :(\n" + difference + " Gold short");
                    }
                }
                else
                {
                    await ctx.RespondAsync("Invalid argument");
                }
            }
        }
    }
}