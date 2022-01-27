using System;

namespace ShacoDiscordBot
{
    public class User
    {
        public ulong ID { get; private set; }
        public string UserName { get; private set; }
        public int Gold { get; set; }
        public int GoldGenerated { get; set; }
        public int CollectionLevel { get; set; }
        public int TimesCollected { get; set; }
        public DateTime LastCollectionTime { get; set; }
        public int CooldownLevel { get; set; }
        public int GoldGifted { get; set; }
        public int GoldReceived { get; set; }
        public int GoldSpent { get; set; }
        public int[] LootBoxInventory { get; set; }
        public int LootBoxesOpened { get; set; }
        public int GoldSpentOnLootBoxes { get; set; }
        public int GoldWonFromLootBoxes { get; set; }
        public int BoxesWonFromLootBoxes { get; set; }
        public int MostGoldWonFromLootBox { get; set; }
        public int CollectionAmount
        {
            get
            {
                if (this.CollectionLevel == 1)
                    return 10;
                else
                    return (5 * this.CollectionLevel) + (this.CollectionLevel * this.CollectionLevel);
            }
            private set { }
        }
        public int CollectionUpgradeCost
        {
            get
            {
                return 25 * (this.CollectionLevel * this.CollectionLevel);
            }
            private set { }
        }
        public int CooldownMaxLevel
        {
            get
            {
                return 20;
            }
            private set { }
        }
        public int CollectionCooldown
        {
            get
            {
                if (this.CooldownLevel == 1)
                {
                    return 600;
                }
                else if (this.CooldownLevel > 1 && this.CooldownLevel <= 20)
                {
                    return 600 - (this.CooldownLevel * 9);
                }
                else if (this.CooldownLevel > 20 && this.CooldownLevel <= 50)
                {
                    return 360 + (2 * (50 - this.CooldownLevel));
                }
                else
                {
                    return 300 + (110 - this.CooldownLevel);
                }
            }
            private set { }
        }
        public int CooldownUpgradeCost
        {
            get
            {
                if (this.CooldownLevel >= 1 && this.CooldownLevel <= 20)
                {
                    return 60 * (this.CooldownLevel * this.CooldownLevel);
                }
                else if (this.CooldownLevel > 20 && this.CooldownLevel <= 50)
                {
                    return 70 * (this.CooldownLevel * this.CooldownLevel);
                }
                else
                {
                    return 80 * (this.CooldownLevel * this.CooldownLevel);
                }
            }
            private set { }
        }
        public User(ulong Id, string username)
        {
            this.ID = Id;
            this.UserName = username;
            this.Gold = 100;
            this.CollectionLevel = 1;
            this.CooldownLevel = 1;
            this.TimesCollected = 0;
            this.LastCollectionTime = DateTime.Now;
            this.GoldGifted = 0;
            this.GoldReceived = 0;
            this.LootBoxInventory = new int[6];
            this.LootBoxInventory[0] = 1;
            this.LootBoxesOpened = 0;
            this.GoldSpentOnLootBoxes = 0;
            this.GoldWonFromLootBoxes = 0;
            this.BoxesWonFromLootBoxes = 0;
            this.MostGoldWonFromLootBox = 0;
        }
    }
}