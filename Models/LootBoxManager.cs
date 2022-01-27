using System;
using DSharpPlus.Entities;

namespace ShacoDiscordBot
{
    public static class LootBoxManager
    {
        public static int GetBoxValue(int rollIndex)
        {
            int value = 0;
            switch (rollIndex)
            {
                case 0:
                    value = 200;
                    break;
                case 1:
                    value = 500;
                    break;
                case 2:
                    value = 1000;
                    break;
                case 3:
                    value = 2000;
                    break;
                case 4:
                    value = 5000;
                    break;
                case 5:
                    value = 10000;
                    break;
                default:
                    break;
            }
            return value;
        }
        public static int RollWeights(int[] weights)
        {
            //Takes an array of item weights and makes a roll on the loot table
            //Returns the index of the item rolled in the array
            int total = 0;
            int index = 0;

            foreach (var weight in weights)
            {
                total += weight;
            }
            var random = new Random();

            int roll = random.Next(0, total + 1);

            for (int i = 0; i < weights.Length; i++)
            {
                if (roll <= weights[i])
                {
                    index = i;
                    break;
                }
                else
                {
                    roll -= weights[i];
                }
            }
            return index;
        }
        public static DiscordEmbedBuilder OpenLootBox(int boxTypeIndex, User user)
        {
            boxTypeIndex--;
            //subtract box from user inventory
            user.LootBoxInventory[boxTypeIndex]--;
            //increase lootboxes opened stat
            user.LootBoxesOpened++;
            var boxData = GameManager.LootBoxes[boxTypeIndex];

            var embed = new DiscordEmbedBuilder
            {
                Title = "Rewards :)",
                Color = DiscordColor.Blurple
            };

            //for the number of rewards in the box, roll the reward type
            //store in array eqaul to the length of numbRewards

            var numRewards = boxData.NumberOfRewards;
            int[] rewardTypes = new int[numRewards];

            for (int i = 0; i < rewardTypes.Length; i++)
            {
                rewardTypes[i] = RollWeights(boxData.RewardTypeWeights);
                //0 = gold reward
                //1 = lootbox reward
            }

            int totalGold = 0;
            for (int i = 0; i < rewardTypes.Length; i++)
            {
                var index = 0;
                if (rewardTypes[i] == 0)
                {
                    //roll for gold
                    index = RollWeights(boxData.GoldRewardWeights);
                    int gold = boxData.GoldRewardValues[index];
                    totalGold += gold;
                    embed.AddField($"{i + 1}. Gold Reward", gold.ToString());
                }
                else
                {
                    //roll for lootbox
                    index = RollWeights(boxData.LootboxRewardWeights);
                    //add new box to inventory
                    user.LootBoxInventory[index]++;
                    //increase boxes won stat
                    user.BoxesWonFromLootBoxes++;
                    embed.AddField($"{i + 1}. LootBox Reward", $"Box Value: {GetBoxValue(index)}");
                }
            }
            embed.WithFooter($"Total Gold Rolled: {totalGold.ToString()}");

            user.Gold += totalGold;
            //increase gold won from lootboxes
            user.GoldWonFromLootBoxes += totalGold;

            //compare most gold won from lootbox
            if (totalGold > user.MostGoldWonFromLootBox)
            {
                user.MostGoldWonFromLootBox = totalGold;
            }

            return embed;
        }
    }
}