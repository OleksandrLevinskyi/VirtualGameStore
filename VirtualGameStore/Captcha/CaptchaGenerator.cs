// Reference: https://github.com/biggray/CaptchaGen.SkiaSharp/blob/master/src/CaptchaGen.SkiaSharp/CaptchaGenerator.cs

using SkiaSharp;
using System.Drawing;
using System.Text;

namespace VirtualGameStore.Captcha
{
    public class CaptchaGenerator
    {
        private const int ImageQuality = 10;

        protected SKColor PaintColor { get; set; }
        protected SKColor BackgroundColor { get; set; }
        protected SKColor NoisePointColor { get; set; }

        protected int ImageWidth { get; set; }
        protected int ImageHeight { get; set; }

        protected string? FontName { get; set; }
        protected int FontSize { get; set; }

        protected double MinDistortion { get; set; }
        protected double MaxDistortion { get; set; }
        protected Func<(int oldX, int oldY, double distortionLevel), (int newX, int newY)>? DistortionFunc { get; set; }
        protected Func<IEnumerable<(int x, int y)>>? NoisePointMapGenFunc { get; set; }

        public static string GenerateCaptchaCode()
        {
            const string allowedCaracters = "2346789ABCDEFGHJKLMNPRTUVWXYZ";

            Random rand = new();
            int maxRand = allowedCaracters.Length - 1;

            StringBuilder sb = new();

            for (int i = 0; i < 4; i++)
            {
                int index = rand.Next(maxRand);
                sb.Append(allowedCaracters[index]);
            }

            return sb.ToString();
        }

        public CaptchaGenerator(
            SKColor paintColor,
            SKColor backgroundColor,
            SKColor noisePointColor,
            int imageWidth = 120,
            int imageHeight = 48,
            string? fontName = null,
            int fontSize = 20,
            Func<(int oldX, int oldY, double distortionLevel), (int newX, int newY)>? distortionFunc = null,
            Func<IEnumerable<(int x, int y)>>? noisePointMapGenFunc = null
        )
        {
            PaintColor = paintColor;
            BackgroundColor = backgroundColor;
            NoisePointColor = noisePointColor;

            ImageWidth = imageWidth;
            ImageHeight = imageHeight;

            FontName = fontName;
            FontSize = fontSize;

            DistortionFunc = distortionFunc;
            NoisePointMapGenFunc = noisePointMapGenFunc;
        }

        public byte[] GenerateImageAsByteArray(
            string captchaCode,
            SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Jpeg, int imageQuality = ImageQuality
        ) => BuildImage(captchaCode)
            .Encode(imageFormat, imageQuality)
            .ToArray();

        public Stream GenerateImageAsStream(
            string captchaCode,
            SKEncodedImageFormat imageFormat = SKEncodedImageFormat.Jpeg, int imageQuality = ImageQuality
        ) => BuildImage(captchaCode)
            .Encode(imageFormat, imageQuality)
            .AsStream();

        protected SKImage BuildImage(string captchaCode)
        {
            var imageInfo = new SKImageInfo(ImageWidth, ImageHeight, SKImageInfo.PlatformColorType, SKAlphaType.Premul);
            using var plainSkSurface = SKSurface.Create(imageInfo);
            var plainCanvas = plainSkSurface.Canvas;
            plainCanvas.Clear(BackgroundColor);

            using (var paintInfo = new SKPaint())
            {
                paintInfo.Typeface = SKTypeface.FromFamilyName(FontName, SKFontStyle.Italic);
                paintInfo.TextSize = FontSize;
                paintInfo.Color = PaintColor;
                paintInfo.IsAntialias = true;

                var xToDraw = (ImageWidth - paintInfo.MeasureText(captchaCode)) / 2;
                var yToDraw = (ImageHeight - FontSize) / 2 + FontSize;
                plainCanvas.DrawText(captchaCode, xToDraw, yToDraw, paintInfo);
            }
            plainCanvas.Flush();

            if (
                null == DistortionFunc
                && null == NoisePointMapGenFunc
            ) return plainSkSurface.Snapshot();

            using var captchaSkSurface = SKSurface.Create(imageInfo);
            var captchaCanvas = captchaSkSurface.Canvas;

            double distortionLevel = 0;
            if (null != DistortionFunc)
            {
                var random = new Random();
                distortionLevel = MinDistortion + (MaxDistortion - MinDistortion) * random.NextDouble();
                if (random.NextDouble() > 0.5) distortionLevel *= -1;
            }
            var plainPixmap = plainSkSurface.PeekPixels();
            for (int x = 0; x < ImageWidth; x++)
            {
                for (int y = 0; y < ImageHeight; y++)
                {
                    var (newX, newY) = null == DistortionFunc ? (x, y) : DistortionFunc((x, y, distortionLevel));
                    var originalPixel = plainPixmap.GetPixelColor(newX, newY);

                    captchaCanvas.DrawPoint(x, y, originalPixel);
                }
            }

            if (null != NoisePointMapGenFunc)
            {
                var noisePointMap = NoisePointMapGenFunc();
                for (var i = 0; i < noisePointMap.Count(); i++)
                {
                    var (x, y) = noisePointMap.ElementAt(i);
                    captchaCanvas.DrawPoint(x, y, NoisePointColor);
                }
            }

            captchaCanvas.Flush();

            return captchaSkSurface.Snapshot();
        }
    }
}
