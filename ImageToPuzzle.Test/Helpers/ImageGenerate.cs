using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageToPuzzle.Test.Helpers
{
	internal class ImageGenerate
	{
        public Stream GenerateGradientImage()
        {
            return GenerateGradientImage(500, 500);
        }

        private Stream GenerateGradientImage(int width, int height)
        {
            using var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Rectangle(0, 0, width, height),
                Color.Blue,
                Color.Red,
                LinearGradientMode.Vertical))
            {
                brush.SetSigmaBellShape(0.5f);
                graphics.FillRectangle(brush, new Rectangle(0, 0, width, height));
            }

            var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            return memoryStream;
        }
    }
}
