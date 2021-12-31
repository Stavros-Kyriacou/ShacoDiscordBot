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
        public int TimesCollected { get; set; }
        public DateTime LastCollectionTime { get; set; }
        public int CooldownLevel { get; set; }
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
                else
                {
                    return 600 - (30 * (this.CooldownLevel - 1));
                }
            }
            private set { }
        }
        public int CooldownUpgradeCost
        {
            get
            {
                return 75 * (this.CooldownLevel * this.CooldownLevel);
            }
            private set { }
        }
        public int GoldGifted { get; set; }
        public int GoldReceived { get; set; }
        public int GoldSpent { get; set; }
        public User(ulong Id, string username)
        {
            this.ID = Id;
            this.UserName = username;
            this.Gold = 0;
            this.CollectionLevel = 1;
            this.CooldownLevel = 1;
            this.TimesCollected = 0;
            this.LastCollectionTime = DateTime.Now;
            this.GoldGifted = 0;
            this.GoldReceived = 0;
        }
    }
}