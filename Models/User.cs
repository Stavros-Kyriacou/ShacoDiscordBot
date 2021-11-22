using System;

namespace ShacoDiscordBot
{
    public class User
    {
        public ulong ID { get; set; }
        public string UserName { get; set; }
        public int Gold { get; set; }
        public int TimesCollected { get; set; }
        public DateTime LastGoldCollectionTime { get; set; }
        public int GoldGifted { get; set; }
        public int GoldReceived { get; set; }
        public User(ulong Id, string username)
        {
            this.ID = Id;
            this.UserName = username;
            this.Gold = 0;
            this.TimesCollected = 0;
            this.LastGoldCollectionTime = DateTime.Now;
            this.GoldGifted = 0;
            this.GoldReceived = 0;
        }
    }
}