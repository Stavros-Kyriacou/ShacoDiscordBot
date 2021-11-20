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
    }
}