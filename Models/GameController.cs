using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.EventArgs;
using System.Text;
using Newtonsoft.Json;

namespace ShacoDiscordBot
{
    public class GameController
    {
        private readonly string saveFile = "save.json";
        List<User> users = new List<User>();
        public int messageEarnings = 10;
        private int startingGold = 0;
        private int goldCooldown = 60;
        public async Task MessageHandler(object sender, MessageCreateEventArgs e)
        {
            if (!e.Author.IsBot)
            {
                User currentUser = new User();
                var currentUserID = e.Author.Id;
                string command = string.Empty;
                int lengthOfCommand = -1;

                if (e.Message.Content.StartsWith("?"))
                {
                    if (e.Message.Content.Contains(' '))
                    {
                        lengthOfCommand = e.Message.Content.IndexOf(' ');
                    }
                    else
                    {
                        lengthOfCommand = e.Message.Content.Length;
                    }

                    command = e.Message.Content.Substring(1, lengthOfCommand - 1).ToLower();
                }


                if (HasProfile(currentUserID))
                {
                    currentUser = GetUserById(currentUserID);
                    await AddGold(currentUser);
                }

                switch (command)
                {
                    case "give":
                        await Give(e);
                        break;
                    case "joingame":
                        await JoinGame(e);
                        break;
                    case "profile":
                        await Profile(e);
                        break;
                    default:
                        break;
                }
                await Save();
            }
        }
        public async Task Load()
        {
            await Task.Run(() =>
            {
                var jsonData = File.ReadAllText(saveFile);
                users = JsonConvert.DeserializeObject<List<User>>(jsonData);
            });
        }
        public async Task Save()
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(users);
            await Task.Run(() =>
            {
                File.WriteAllText(saveFile, jsonString);
            });
        }
        public async Task AddGold(User user)
        {
            await Task.Run(() =>
            {
                TimeSpan duration = DateTime.Now - user.LastGoldCollectionTime;
                if (duration.TotalSeconds >= goldCooldown)
                {
                    user.Gold += messageEarnings;
                    user.TimesCollected++;
                    user.LastGoldCollectionTime = DateTime.Now;
                }
            });
        }
        public async Task JoinGame(MessageCreateEventArgs e)
        {
            await Task.Run(() =>
            {
                if (!HasProfile(e.Message.Author.Id))
                {
                    if (this.users != null)
                    {
                        this.users.Add(new User { ID = e.Message.Author.Id, UserName = e.Author.Username, Gold = startingGold, TimesCollected = 1, LastGoldCollectionTime = DateTime.Now });
                    }
                    else
                    {
                        this.users = new List<User>();
                        this.users.Add(new User { ID = e.Message.Author.Id, UserName = e.Author.Username, Gold = startingGold, TimesCollected = 1, LastGoldCollectionTime = DateTime.Now });
                    }
                }
                else
                {
                    e.Message.RespondAsync(e.Author.Username + ", you already have a profile");
                }
            });
        }
        public async Task Profile(MessageCreateEventArgs e)
        {
            await Task.Run(() =>
            {
                var u = GetUserById(e.Message.Author.Id);
                if (u != null)
                {
                    e.Message.RespondAsync($"Gold: {u.Gold}    Times Collected: {u.TimesCollected} \nLast Collection TIme: {u.LastGoldCollectionTime}\nNext Collection Time: {u.LastGoldCollectionTime.AddSeconds(goldCooldown)}");
                }
            });
        }
        public async Task Give(MessageCreateEventArgs e)
        {
            await Task.Run(() =>
            {
                var receivers = e.MentionedUsers;
                ulong receiverId = 0;
                int amount = GetTransferAmount(e.Message.Content);

                if (receivers != null)
                {
                    receiverId = receivers[0].Id;
                }

                User sendingUser = new User();
                User receivingUser = new User();

                foreach (var u in users)
                {
                    if (u.ID == e.Message.Author.Id)
                    {
                        sendingUser = u;
                    }
                    else if (u.ID == receiverId)
                    {
                        receivingUser = u;
                    }
                }

                if (sendingUser.Gold >= amount)
                {
                    sendingUser.Gold -= amount + messageEarnings;
                    receivingUser.Gold += amount;

                    e.Message.RespondAsync($"{sendingUser.UserName} gave {receivingUser.UserName} {amount} gold");
                }
                else
                {
                    e.Message.RespondAsync("Not enough funds");
                }
            });
        }
        public bool HasProfile(ulong currentUserId)
        {
            bool hasProfile = false;
            if (users != null)
            {
                foreach (var u in users)
                {
                    if (u.ID == currentUserId)
                    {
                        hasProfile = true;
                        break;
                    }
                }
            }
            return hasProfile;
        }
        public int GetTransferAmount(string message)
        {
            int result = 0;
            var split = message.Split(' ');

            if (int.TryParse(split[1], out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }
        public User GetUserById(ulong id)
        {
            foreach (var u in users)
            {
                if (u.ID == id)
                {
                    return u;
                }
            }
            return null;
        }
    }
}