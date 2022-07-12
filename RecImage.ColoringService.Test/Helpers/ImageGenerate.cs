using System.Drawing;
using System.IO;

namespace RecImage.ColoringService.Test.Helpers;

internal class ImageGenerate
{
    public static Stream GenerateGradientImage()
    {
        return GenerateGradientImage(500, 500);
    }

    private static Stream GenerateGradientImage(int width, int height)
    {
        using var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);

        using (var brush = new LinearGradientBrush(new Rectangle(0, 0, width, height),
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