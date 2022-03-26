using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System.Drawing.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
//dotnet add package System.Drawing.Common to use the drawing stuff

namespace ShacoDiscordBot
{
    public class TestCommands : VoiceNextCommands
    {
        [Command("test")]
        public async Task Test(CommandContext ctx)
        {
            if (ctx.Message.Author.Id == 257448897746698241)
            {
                var user = GameManager.GetUserById(257448897746698241);
                Font titleFont = new Font("Arial", 20, FontStyle.Regular, GraphicsUnit.Pixel);
                Font font = new Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel);
                Bitmap bitmap = new Bitmap(1, 1);
                Graphics graphics = Graphics.FromImage(bitmap);
                bitmap = new Bitmap(bitmap, new Size(500, 200));
                graphics = Graphics.FromImage(bitmap);

                // var xPadding = 10;
                var yPadding = 3;

                var title = $"{user.UserName}'s Profile";
                var titleSize = graphics.MeasureString(title, titleFont);

                var line1 = $"Gold: {user.Gold}    Next Collection In: {user.CollectionTimeRemaining()}";
                var line1Size = graphics.MeasureString(line1, font);

                var line2 = $"Collection Amount: {user.CollectionAmount}    Level: {user.CollectionLevel}    Upgrade Cost: {user.CollectionUpgradeCost}";
                var line2Size = graphics.MeasureString(line2, font);

                var span = user.LastCollectionTime.AddSeconds(user.CollectionCooldown) - user.LastCollectionTime;
                var cooldown = $"{span.Duration().Minutes}m {span.Duration().Seconds}s";
                var line3 = $"Collection CD: {cooldown}    Level: {user.CooldownLevel}    Upgrade Cost: {user.CooldownUpgradeCost}";
                var line3Size = graphics.MeasureString(line3, font);


                // int width = (int)graphics.MeasureString(text, font).Width;
                // int height = (int)graphics.MeasureString(text, font).Height;

                graphics.Clear(Color.Black);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                var whiteBrush = new SolidBrush(Color.White);

                graphics.DrawString(title, titleFont, whiteBrush, 0, 0);
                graphics.DrawString(line1, titleFont, whiteBrush, 0, titleSize.Height + yPadding);
                graphics.DrawString(line2, titleFont, whiteBrush, 0, titleSize.Height + yPadding + line1Size.Height + yPadding);
                graphics.DrawString(line3, titleFont, whiteBrush, 0, titleSize.Height + yPadding + line1Size.Height + yPadding + line2Size.Height + yPadding);

                graphics.Flush();
                graphics.Dispose();
                string fileName = "test.jpg";
                bitmap.Save(fileName, ImageFormat.Jpeg);

                // await ImageGenerator.Draw(text);
                var message = new DiscordMessageBuilder();

                var fs = File.OpenRead("test.jpg");
                message.WithFile(fs);

                await ctx.Channel.SendMessageAsync(message);
                //NEED TO CLOSE THE FILESTREAM BEFORE OPENING ON THE SAME FILE THIS FIXED THE ISSUE
                fs.Close();
            }
            else
            {
                await ctx.RespondAsync(Shaco.PermissionMessage);
            }
        }
    }
}