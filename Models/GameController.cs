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
                var user = GameManager.GetUserById(e.Author.Id);
                if (user != null)
                {
                    await GameManager.AddGold(user);
                }
            }
        }
    }
}