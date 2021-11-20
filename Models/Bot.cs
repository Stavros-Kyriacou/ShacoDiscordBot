using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.VoiceNext;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ShacoDiscordBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public VoiceNextExtension Voice { get; private set; }
        public GameController GameController {get; private set;}
        // public List<User> users = new List<User>();
        public async Task RunAsync()
        {
            var json = "";

            using (var fileStream = File.OpenRead("config.json"))
            {
                using (var streamReader = new StreamReader(fileStream, new UTF8Encoding(false)))
                {
                    json = await streamReader.ReadToEndAsync().ConfigureAwait(false);
                }
            }

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };
            this.GameController = new GameController();

            this.Client = new DiscordClient(config);

            this.Client.MessageCreated += GameController.MessageHandler;
            this.Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = false
            };

            this.Commands = Client.UseCommandsNext(commandsConfig);

            this.Commands.RegisterCommands<RandomCommands>();
            this.Commands.RegisterCommands<VoiceCommands>();
            this.Commands.RegisterCommands<GameCommands>();

            // GameCommands commands = new GameCommands(this.GameController);

            this.Voice = Client.UseVoiceNext();

            await this.GameController.Load();

            await this.Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(object sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}