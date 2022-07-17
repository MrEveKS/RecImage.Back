using System.Drawing;
using ImageMagick;
using RecImage.ColoringService.Enums;
using RecImage.ColoringService.Models;

namespace RecImage.ColoringService.Services;

internal sealed class ImageConverter : IImageConverter, IDisposable
{
    private bool _disposedValue;

    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<ColorPoints> ConvertToColorPoints(Stream imageStream,
        ConvertOptions options)
    {
        var image = await ImageStreamConvert(imageStream, options);

        return ImageConvert(image, options);
    }

    private async Task<MagickImage> ImageStreamConvert(Stream imageStream, ConvertOptions options)
    {
        imageStream.Position = 0;
        var image = new MagickImage(imageStream);
        var size = CalculateNewSize(image, options.Size);

        var image256 = options.ColorStep switch
        {
            ColorStep.Middle or ColorStep.Big or ColorStep.VeryBig when options.Colored => ResizeImage(
                ConvertTo256(image), size),
            ColorStep.Middle or ColorStep.Big or ColorStep.VeryBig when !options.Colored => GrayScale(
                ResizeImage(ConvertTo256(image), size)),
            ColorStep.Small or ColorStep.VerySmall or _ when options.Colored => ConvertTo256(ResizeImage(image, size)),
            ColorStep.Small or ColorStep.VerySmall or _ when !options.Colored => ConvertTo256(
                GrayScale(ResizeImage(image, size)))
        };

        await imageStream.DisposeAsync();

        return image256;
    }

    private static ColorPoints ImageConvert(MagickImage image, ConvertOptions options)
    {
        var colorStep = (int)options.ColorStep;
        const int pixelSize = 1;
        const int separatePointSize = pixelSize * pixelSize;

        var colors = new Dictionary<string, int>();

        var heightWithSeparate = image.Height - image.Height % pixelSize;
        var widthWithSeparate = image.Width - image.Width % pixelSize;

        var cells = new List<List<int>>(heightWithSeparate);

        using var pixels = image.GetPixelsUnsafe();

        for (var colIndex = 0; colIndex < heightWithSeparate; colIndex += pixelSize)
        {
            var row = new List<int>(widthWithSeparate);

            for (var rowIndex = 0; rowIndex < widthWithSeparate; rowIndex += pixelSize)
            {
                var red = 0;
                var green = 0;
                var blue = 0;

                for (var i = rowIndex; i < rowIndex + pixelSize; i++)
                {
                    for (var j = colIndex; j < colIndex + pixelSize; j++)
                    {
                        var pixelColor = pixels.GetPixel(rowIndex, colIndex);
                        red += pixelColor[0]; //pixelColor.R;
                        green += pixelColor[1]; //pixelColor.G;
                        blue += pixelColor[2]; //pixelColor.B;
                    }
                }

                var separatedRed = red / separatePointSize;
                var separatedGreen = green / separatePointSize;
                var separatedBlue = blue / separatePointSize;

                var color = GetNewColor(separatedRed, separatedGreen, separatedBlue, colorStep);
                var webColor = ColorTranslator.ToHtml(color);
                int index;

                if (colors.ContainsKey(webColor))
                {
                    index = colors[webColor];
                }
                else
                {
                    index = colors.Count + 1;
                    colors.Add(webColor, index);
                }

                row.Add(index);
            }

            cells.Add(row);
        }

        return new ColorPoints
        {
            Cells = cells,
            CellsColor = ToCellsColor(colors)
        };
    }

    private static MagickImage ConvertTo256(MagickImage image)
    {
        image.Quantize(new QuantizeSettings { Colors = 256, DitherMethod = DitherMethod.No });
        return image;
    }

    private void Dispose(bool disposing)
    {
        if (_disposedValue) return;

        if (disposing)
        {
            GC.Collect(GC.MaxGeneration);
        }

        _disposedValue = true;
    }

    ~ImageConverter()
    {
        Dispose(false);
    }

    private static Color GetNewColor(int separatedRed,
        int separatedGreen,
        int separatedBlue,
        int colorStep)
    {
        separatedRed = UpdateColor(separatedRed, colorStep);
        separatedGreen = UpdateColor(separatedGreen, colorStep);
        separatedBlue = UpdateColor(separatedBlue, colorStep);

        return Color.FromArgb(separatedRed, separatedGreen, separatedBlue);
    }

    private static int UpdateColor(int color,
        int colorStep)
    {
        const int white = 248;
        const int lightWhite = 250;

        if (color > white)
        {
            color = lightWhite;
        }
        else
        {
            color -= color % colorStep;
        }

        return color;
    }

    private static SizeF CalculateNewSize(MagickImage image, double maxWidth)
    {
        var imageWidth = image.Width;
        var cof = maxWidth / imageWidth;
        var newWidth = (int)maxWidth;
        var newHeight = (int)(cof * image.Height);

        return new SizeF
        {
            Height = newWidth,
            Width = newHeight
        };
    }

    private static MagickImage ResizeImage(MagickImage image, SizeF newSize)
    {
        var resizeWidth = newSize.Width / (double)image.Width;
        var resizeHeight = newSize.Height / (double)image.Height;

        if (resizeWidth < resizeHeight)
        {
            var newWidth = (int)(resizeWidth * image.Width);
            var newHeight = (int)(resizeWidth * image.Height);
            image.Resize(newWidth, newHeight);
        }
        else
        {
            var newWidth = (int)(resizeHeight * image.Width);
            var newHeight = (int)(resizeHeight * image.Height);
            image.Resize(newWidth, newHeight);
        }

        return image;
    }

    private static MagickImage GrayScale(MagickImage image)
    {
        image.Grayscale();
        return image;
    }

    private static Dictionary<int, string> ToCellsColor(Dictionary<string, int> colors)
    {
        var result = new Dictionary<int, string>(colors.Count);

        foreach (var (key, value) in colors)
        {
            result.Add(value, key);
        }

        return result;
    }
}