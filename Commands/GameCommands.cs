using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Generic;
using DSharpPlus.Entities;
using System;
using System.Linq;
using DSharpPlus.Interactivity.Extensions;

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
        [Description("Display user profile information")]
        public async Task Profile(CommandContext ctx, [Description("Leave empty for short profile, type \"?profile full\" to display full profile")] params string[] args)
        {
            DiscordEmbedBuilder embed = new DiscordEmbedBuilder();
            var user = GameManager.GetUserById(ctx.Message.Author.Id);

            if (args.Length == 0)
            {
                embed = UserProfile(user)
                        .WithThumbnail(ctx.Message.Author.AvatarUrl, 100, 100);
            }
            else if (args[0] == "full")
            {
                embed = UserProfileFull(user)
                        .WithThumbnail(ctx.Message.Author.AvatarUrl, 100, 100);
            }
            await ctx.RespondAsync(embed: embed);
        }

        [Command("findprofile")]
        public async Task FindProfile(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();

            var embed = new DiscordEmbedBuilder
            {
                Title = "Find User Profile",
                Color = DiscordColor.Green
            };
            var users = GameManager.Users;

            for (int i = 0; i < users.Count; i++)
            {
                embed.AddField($"{i + 1}. {users[i].UserName}", "----------------");
            }

            await ctx.RespondAsync(embed: embed);
            var message = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Message.Author);


            if (int.TryParse(message.Result.Content, out int index))
            {
                if (index > 0 && index <= users.Count)
                {
                    var user = GameManager.GetUserById(users[index - 1].ID);
                    var profileEmbed = UserProfile(user);
                    await ctx.RespondAsync(embed: profileEmbed);
                }
            }
        }

        [Command("allprofiles")]
        public async Task AllProfiles(CommandContext ctx)
        {
            if (ctx.Message.Author.Id == 257448897746698241)
            {
                foreach (var user in GameManager.Users)
                {
                    await ctx.RespondAsync(UserProfileFull(user));
                }
            }
            else
            {
                await ctx.RespondAsync(Shaco.PermissionMessage);
            }
        }

        [Command("gift")]
        public async Task Gift(CommandContext ctx, int amount, DiscordMember mention)
        {
            if (amount < 0)
            {
                amount *= -1;
            }
            
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
        
        public DiscordEmbedBuilder UserProfile(User user)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{user.UserName}'s Profile",
                Color = DiscordColor.Red
            };

            var span = user.LastCollectionTime.AddSeconds(user.CollectionCooldown) - user.LastCollectionTime;
            var cooldown = $"{span.Duration().Minutes}m {span.Duration().Seconds}s";

            embed.AddField("Gold", user.Gold.ToString(), true)
                    .AddField("Next Collection TIme", user.LastCollectionTime.AddSeconds(user.CollectionCooldown).ToString(), true)
                    .AddField("\u200b", "\u200b") //empty space

                    .AddField("Collection Amount", user.CollectionAmount.ToString(), true)
                    .AddField("Level", user.CollectionLevel.ToString(), true)
                    .AddField("Upgrade Cost", user.CollectionUpgradeCost.ToString(), true)
                    .AddField("\u200b", "\u200b") //empty space

                    .AddField("Collection Cooldown", cooldown, true)
                    .AddField("Level", user.CooldownLevel.ToString(), true)
                    .AddField("Upgrade Cost", user.CooldownUpgradeCost.ToString(), true)
                    ;
            embed.WithFooter(Shaco.RandomVoiceLine().Description);
            return embed;
        }

        public DiscordEmbedBuilder UserProfileFull(User user)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{user.UserName}'s Profile",
                Color = DiscordColor.Red
            };

            var span = user.LastCollectionTime.AddSeconds(user.CollectionCooldown) - user.LastCollectionTime;
            var cooldown = $"{span.Duration().Minutes}m {span.Duration().Seconds}s";

            embed.AddField("Gold", user.Gold.ToString())
                    .AddField("Gold Spent", user.GoldSpent.ToString(), true)
                    .AddField("Gold Gifted", user.GoldGifted.ToString(), true)
                    .AddField("Gold Received", user.GoldReceived.ToString(), true)
                    .AddField("\u200b", "\u200b") //empty space

                    .AddField("Next Collection TIme", user.LastCollectionTime.AddSeconds(user.CollectionCooldown).ToString(), true)
                    .AddField("Last Collection Time", user.LastCollectionTime.ToString(), true)
                    .AddField("Times Collected", user.TimesCollected.ToString(), true)
                    .AddField("\u200b", "\u200b") //empty space

                    .AddField("Collection Amount", user.CollectionAmount.ToString(), true)
                    .AddField("Level", user.CollectionLevel.ToString(), true)
                    .AddField("Upgrade Cost", user.CollectionUpgradeCost.ToString(), true)
                    .AddField("\u200b", "\u200b") //empty space

                    .AddField("Collection Cooldown", cooldown, true)
                    .AddField("Level", user.CooldownLevel.ToString(), true)
                    .AddField("Upgrade Cost", user.CooldownUpgradeCost.ToString(), true)
                    ;
            embed.WithFooter(Shaco.RandomVoiceLine().Description);
            return embed;
        }

        public DiscordEmbedBuilder UserShop(User user)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = $"{user.UserName}'s Shop",
                Color = DiscordColor.Yellow
            };

            var span = user.LastCollectionTime.AddSeconds(user.CollectionCooldown) - user.LastCollectionTime;
            var cooldown = $"{span.Duration().Minutes}m {span.Duration().Seconds}s";

            embed.AddField("1. Collection Amount Upgrade", "\u200b") //empty space
                    .AddField("Cost", user.CollectionUpgradeCost.ToString(), true)
                    .AddField("Current Level", user.CollectionLevel.ToString(), true)
                    .AddField("Current Collection Amount", user.CollectionAmount.ToString(), true)
                    .AddField("\u200b", "\u200b")

                    .AddField("2. Collection Cooldown Upgrade", "\u200b")
                    .AddField("Cost", user.CooldownUpgradeCost.ToString(), true)
                    .AddField("Current Level", user.CooldownLevel.ToString(), true)
                    .AddField("Current Collection Cooldown", cooldown, true)
                    ;
            embed.WithFooter(Shaco.RandomVoiceLine().Description);
            return embed;
        }

        [Command("shop")]
        [Description("View upgrades available for purchase. Use ?buy {shopNumber} to purchase upgrades")]
        public async Task Shop(CommandContext ctx)
        {
            var user = GameManager.GetUserById(ctx.Message.Author.Id);
            var embed = UserShop(user);
            await ctx.RespondAsync(embed: embed);
        }

        [Command("buy")]
        [Description("Purchase gold collection and cooldown upgrades. Use ?shop to view upgrades available for purchase")]
        public async Task Buy(CommandContext ctx, [Description("The number of the shop item. Use ?shop to view upgrades available for purchase")] int shopNumber)
        {
            var user = GameManager.GetUserById(ctx.Message.Author.Id);

            switch (shopNumber)
            {
                case 1:
                    if (user.Gold >= user.CollectionUpgradeCost)
                    {
                        user.Gold -= user.CollectionUpgradeCost;
                        user.GoldSpent += user.CollectionUpgradeCost;
                        user.CollectionLevel++;

                        var embed = new DiscordEmbedBuilder
                        {
                            Title = "Gold Collection Upgrade Purchased!",
                            Color = DiscordColor.Green
                        };

                        embed.AddField("Gold", user.Gold.ToString(), true)
                                .AddField("Collection Amount", user.CollectionAmount.ToString(), true)
                                .AddField("Level", user.CollectionLevel.ToString(), true)
                                .AddField("Upgrade Cost", user.CollectionUpgradeCost.ToString(), true);

                        await ctx.RespondAsync(embed: embed);
                        await GameManager.Save();
                    }
                    else
                    {
                        var difference = user.CollectionUpgradeCost - user.Gold;
                        await ctx.RespondAsync("Not enough funds :(\n" + difference + " Gold short");
                    }
                    break;
                case 2:
                    if (user.CooldownLevel >= user.CooldownMaxLevel)
                    {
                        await ctx.RespondAsync("Cooldown Upgrade at Max Level!\nNo Further Upgrades Available");
                        break;
                    }
                    if (user.Gold >= user.CooldownUpgradeCost)
                    {
                        user.Gold -= user.CooldownUpgradeCost;
                        user.GoldSpent += user.CooldownUpgradeCost;
                        user.CooldownLevel++;

                        var embed = new DiscordEmbedBuilder
                        {
                            Title = "Gold Collection Cooldown Upgrade Purchased!",
                            Color = DiscordColor.Green
                        };
                        var span = user.LastCollectionTime.AddSeconds(user.CollectionCooldown) - user.LastCollectionTime;
                        var cooldown = $"{span.Duration().Minutes}m {span.Duration().Seconds}s";

                        embed.AddField("Gold", user.Gold.ToString(), true)
                                .AddField("Collection Cooldown", cooldown, true)
                                .AddField("Level", user.CooldownLevel.ToString(), true)
                                .AddField("Upgrade Cost", user.CooldownUpgradeCost.ToString(), true);

                        await ctx.RespondAsync(embed: embed);
                        await GameManager.Save();
                    }
                    else
                    {
                        var difference = user.CooldownUpgradeCost - user.Gold;
                        await ctx.RespondAsync("Not enough funds :(\n" + difference + " Gold short");
                    }
                    break;
                default:
                    break;
            }
        }

        [Command("leaderboard")]
        [Description("Display leaderboards for various stats")]
        public async Task Leaderboard(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Current Gold Leaderboard",
                Color = DiscordColor.Green
            };
            var leaders = GameManager.Users.OrderByDescending(u => u.Gold).ToList();

            for (int i = 0; i < leaders.Count; i++)
            {
                embed.AddField($"{i + 1}. {leaders[i].UserName}", leaders[i].Gold.ToString());
            }
            await ctx.RespondAsync(embed: embed);
        }

        [Command("leaderboard")]
        [Description("Display leaderboards for various stats. Enter a stat filter to change leaderboard type")]
        public async Task Leaderboard(CommandContext ctx, [Description("Leaderboard stat filters: generated, spent, gifted, received, collected")] string stat)
        {
            List<User> leaders = new List<User>();

            var embed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green
            };

            switch (stat)
            {
                case "generated":
                    leaders = GameManager.Users.OrderByDescending(u => u.GoldGenerated).ToList();
                    embed.Title = "Gold Generated Leaderboard";
                    for (int i = 0; i < leaders.Count; i++)
                    {
                        embed.AddField($"{i + 1}. {leaders[i].UserName}", leaders[i].GoldGenerated.ToString());
                    }
                    break;
                case "spent":
                    leaders = GameManager.Users.OrderByDescending(u => u.GoldSpent).ToList();
                    embed.Title = "Gold Spent Leaderboard";
                    for (int i = 0; i < leaders.Count; i++)
                    {
                        embed.AddField($"{i + 1}. {leaders[i].UserName}", leaders[i].GoldSpent.ToString());
                    }
                    break;
                case "gifted":
                    leaders = GameManager.Users.OrderByDescending(u => u.GoldGifted).ToList();
                    embed.Title = "Gold Gifted Leaderboard";
                    for (int i = 0; i < leaders.Count; i++)
                    {
                        embed.AddField($"{i + 1}. {leaders[i].UserName}", leaders[i].GoldGifted.ToString());
                    }
                    break;
                case "received":
                    leaders = GameManager.Users.OrderByDescending(u => u.GoldReceived).ToList();
                    embed.Title = "Gold Received Leaderboard";
                    for (int i = 0; i < leaders.Count; i++)
                    {
                        embed.AddField($"{i + 1}. {leaders[i].UserName}", leaders[i].GoldReceived.ToString());
                    }
                    break;
                case "collected":
                    leaders = GameManager.Users.OrderByDescending(u => u.TimesCollected).ToList();
                    embed.Title = "Times Collected Leaderboard";
                    for (int i = 0; i < leaders.Count; i++)
                    {
                        embed.AddField($"{i + 1}. {leaders[i].UserName}", leaders[i].TimesCollected.ToString());
                    }
                    break;
                default:
                    break;
            }

            await ctx.RespondAsync(embed: embed);
        }
    }
}