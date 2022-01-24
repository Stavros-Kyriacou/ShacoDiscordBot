namespace ShacoDiscordBot
{
    public class Lootbox
    {
        public int Cost { get; set; }
        public int NumberOfRewards { get; set; }
        public int[] RewardTypeWeights { get; set; }
        public int[] GoldRewardValues { get; set; }
        public int[] GoldRewardWeights { get; set; }
        public int[] LootboxRewardWeights { get; set; }
    }
}