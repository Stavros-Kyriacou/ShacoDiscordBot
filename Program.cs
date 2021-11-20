using System;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.WebSocket;
using DSharpPlus.VoiceNext;

namespace ShacoDiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new Bot();
            bot.RunAsync().GetAwaiter().GetResult();
        }
    }
}