using System;
using System.IO;
using System.Text;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Zibma.Captcha.NetCore
{
    public class CaptchaHelper
    {

        public static Color LineColor { get; set; } = Color.Gray;
        public static int NoumberOfLines { get; set; } = 5;
        public static Color TextColor { get; set; } = Color.Gray;
        public static Rgba32 BackgroundColor { get; set; } = Color.White;
        public static string FontPath { get; set; } = "Fonts/Arial/arial.ttf";


        public static MemoryStream BuildImage(string captchaCode, int imageWidth, int imageHeight, int fontSize)
        {
            var image = new Image<Rgba32>(imageWidth, imageHeight, BackgroundColor);

            DrawLines(ref image, LineColor, 2, NoumberOfLines, imageWidth, imageHeight);

            DrawText(ref image, captchaCode, TextColor, fontSize);

            using (var stream = new MemoryStream())
            {
                image.Save(stream, new PngEncoder());
                return stream;
            }
        }

        private static void DrawLines(ref Image<Rgba32> image, Color color, int thickness, int noOfLines, int imageWidth, int imageHeight)
        {
            Random random = new Random();

            for (int i = 0; i < noOfLines; i++)
            {
                int startX = random.Next(imageWidth);
                int startY = random.Next(imageHeight);
                int endX = random.Next(imageWidth);
                int endY = random.Next(imageHeight);

                DrawLine(ref image, color, thickness, startX, startY, endX, endY);
            }
        }
        private static void DrawLine(ref Image<Rgba32> image, Color color, int thickness, int startX, int startY, int endX, int endY)
        {
            image.Mutate(x => x.DrawLines(color, thickness, new PointF(startX, startY), new PointF(endX, endY)));
        }

        private static void DrawText(ref Image<Rgba32> image, string data, Color color, int fontSize)
        {
            image.Mutate(x => x.DrawText(data, GetFont(fontSize), color, new PointF(1, 1)));
        }

        //private static void DrawRectangle(ref Image<Rgba32> image)
        //{
        //    image.Mutate(x => x.DrawPolygon(Color.Red, 10, new PointF(100, 100), new PointF(200, 100), new PointF(200, 200), new PointF(100, 200)));
        //}


        private static FontFamily? fontFamily = null;
        private static Font GetFont(int fontSize)
        {
            if (!fontFamily.HasValue)
            {
                fontFamily = new FontCollection().Add(FontPath);
            }

            // Create a Font instance with a specific size
            var textFont = new Font(fontFamily.Value, fontSize, FontStyle.Italic);

            return textFont;
        }







        public static string CreateCode(int digitCount = 5)
        {
            // Not Included: I, O, 0
            char[] chars = {'A','B','C','D','E','F','G','H','J','K','L','M','N','P','Q','R','S','T','U','V','W','X','Y','Z',
                '1','2','3','4','5','6','7','8','9'};
            StringBuilder sb = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < digitCount; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }
            return sb.ToString();
        }

    }
}
