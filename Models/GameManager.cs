
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShacoDiscordBot
{
    public static class GameManager
    {
        //Setup
        private static string saveFile = "save.json";
        public static List<User> Users { get; private set; }

        //Game Variables
        public static int MessageEarnings { get; private set; }

        public static int StartingGold { get; private set; }
        public static int CollectionCooldown { get; private set; }
        static GameManager()
        {
            MessageEarnings = 10;
            StartingGold = 0;
            CollectionCooldown = 60;
            Users = new List<User>();
        }
        public static async Task Load()
        {
            await Task.Run(() =>
            {
                var jsonData = File.ReadAllText(saveFile);
                Users = JsonConvert.DeserializeObject<List<User>>(jsonData);
                if (Users == null)
                {
                    Users = new List<User>();
                }
            });
        }
        public static async Task Save()
        {
            await Task.Run(() =>
            {
                string jsonString = System.Text.Json.JsonSerializer.Serialize(Users);
                File.WriteAllText(saveFile, jsonString);
            });
        }
        public static async Task AddGold(User user)
        {
            await Task.Run(() =>
            {
                TimeSpan duration = DateTime.Now - user.LastGoldCollectionTime;
                if (duration.TotalSeconds >= CollectionCooldown)
                {
                    user.Gold += MessageEarnings;
                    user.TimesCollected++;
                    user.LastGoldCollectionTime = DateTime.Now;
                }
            });
            await Save();
        }
        public static User GetUserById(ulong userId)
        {
            foreach (var u in Users)
            {
                if (u.ID == userId)
                {
                    return u;
                }
            }
            return null;
        }
        public static bool HasProfile(ulong userId)
        {
            if (Users != null)
            {
                foreach (var u in Users)
                {
                    if (u.ID == userId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}