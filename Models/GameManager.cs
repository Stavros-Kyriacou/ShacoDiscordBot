
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShacoDiscordBot
{
    public static class GameManager
    {
        public static string test = "test string";
        public static List<User> users = new List<User>();
        private static string saveFile = "save.json";
        public static async Task Load()
        {
            await Task.Run(() =>
            {
                var jsonData = File.ReadAllText(saveFile);
                users = JsonConvert.DeserializeObject<List<User>>(jsonData);
            });
        }
        public static async Task Save()
        {
            string jsonString = System.Text.Json.JsonSerializer.Serialize(users);
            await Task.Run(() =>
            {
                File.WriteAllText(saveFile, jsonString);
            });
        }
        public static async Task AddGold()
        {
            await Task.Run(() =>
            {

            });
        }
        public static async Task JoinGame()
        {
            await Task.Run(() =>
            {

            });
        }
        public static async Task Profile()
        {
            await Task.Run(() =>
            {

            });
        }
        public static async Task Give()
        {
            await Task.Run(() =>
            {

            });
        }
        public static int GetInt()
        {
            return 1;
        }
    }
}