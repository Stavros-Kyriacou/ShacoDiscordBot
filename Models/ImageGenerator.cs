using System.Drawing.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace ShacoDiscordBot
{
    public static class ImageGenerator
    {
        private static string fileName = "test.jpg";
        private static Bitmap Bitmap;
        private static Font Font;
        private static Graphics Graphics;
        static ImageGenerator()
        {
            Bitmap = new Bitmap(1, 1);
            Bitmap = new Bitmap(200, 200);
            Font = new Font("Arial", 25, FontStyle.Regular, GraphicsUnit.Pixel);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        }
        // public static async Task Draw(string text)
        // {
        //     await Task.Run(() =>
        //     {
        //         Graphics.Clear(Color.White);
        //         Graphics.DrawString(text, Font, new SolidBrush(Color.Red), 0, 0);
        //         Bitmap.Save(fileName, ImageFormat.Jpeg);
        //     });
        // }
        public static async Task Draw(string text)
        {
            await Task.Run(() =>
            {
                Bitmap bitmap = new Bitmap(1, 1);
                Font font = new Font("Arial", 25, FontStyle.Regular, GraphicsUnit.Pixel);
                Graphics graphics = Graphics.FromImage(bitmap);
                int width = (int)graphics.MeasureString(text, font).Width;
                int height = (int)graphics.MeasureString(text, font).Height;
                bitmap = new Bitmap(bitmap, new Size(width, height));
                graphics = Graphics.FromImage(bitmap);
                graphics.Clear(Color.White);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                graphics.DrawString(text, font, new SolidBrush(Color.FromArgb(255, 0, 0)), 0, 0);
                graphics.Flush();
                graphics.Dispose();
                string fileName = "test.jpg";
                bitmap.Save(fileName, ImageFormat.Jpeg);
            });
        }
    }
}