using DSharpPlus.CommandsNext;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Generic;

namespace ShacoDiscordBot
{
    public class GameCommands : BaseCommandModule
    {
        // private GameController gameController;
        // public GameCommands(GameController gc)
        // {
        //     this.gameController = gc;
        // }
        [Command("test")]
        public async Task Test(CommandContext ctx)
        {
            // await ctx.Channel.SendMessageAsync(gameController.messageEarnings.ToString());
            // await ctx.Channel.SendMessageAsync(GameManager.GetInt().ToString());
            await GameManager.Load();
            var u = GameManager.users[0];
            await ctx.RespondAsync($"Gold: {u.Gold}    Times Collected: {u.TimesCollected} \nLast Collection TIme: {u.LastGoldCollectionTime}\nNext Collection Time: {u.LastGoldCollectionTime.AddSeconds(60)}");
        }
    }
}