using ImageConverter.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ImageConverter
{
	public class ImageConverter : IImageConverter
	{
		public ImageConverter()
		{
		}

		public RecColor ConvertToChars(Stream imageStream, ConvertOptions options)
		{
			var colorStep = (int)options.ColorStep;
			var pixelSize = 1;
			var pQ = pixelSize * pixelSize;

			var colors = new Dictionary<string, int>();

			using Bitmap image = ((Bitmap)Bitmap.FromStream(imageStream));
			var size = CalculateNewSize(image, options.Size);
			var imageResized = ResizeImage(image, size);
			if (!options.Colored)
			{
				imageResized = Grayscale(imageResized);
			}

			var height = imageResized.Height - imageResized.Height % pixelSize;
			var width = imageResized.Width - imageResized.Width % pixelSize;

			var colCell = new List<List<int>>(height);
			for (int indexHeight = 0; indexHeight < height; indexHeight += pixelSize)
			{
				var row = new List<int>(width);
				for (int indexWidth = 0; indexWidth < width; indexWidth += pixelSize)
				{
					var r = 0;
					var g = 0;
					var b = 0;
					var rowIndex = indexWidth;
					var colIndex = indexHeight;

					for (int i = rowIndex; i < rowIndex + pixelSize; i++)
					{
						for (int j = colIndex; j < colIndex + pixelSize; j++)
						{
							Color pixelColor = imageResized.GetPixel(rowIndex, colIndex);
							r += pixelColor.R;
							g += pixelColor.G;
							b += pixelColor.B;
						}
					}

					var nR = r / pQ;
					var nG = g / pQ;
					var nB = b / pQ;

					var color = GetNewColor(nR, nG, nB, colorStep);
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
				colCell.Add(row);
			}
			imageResized.Dispose();
			return new RecColor()
			{
				Cells = colCell,
				CellsColor = ToCellsColor(colors)
			};
		}

		private Color GetNewColor(int nR, int nG, int nB, int colorStep)
		{
			nR = UpdateColor(nR, colorStep);
			nG = UpdateColor(nG, colorStep);
			nB = UpdateColor(nB, colorStep);

			return Color.FromArgb(nR, nG, nB);
		}

		private int UpdateColor(int color, int colorStep)
		{
			var white = 248;
			var lightWhite = 250;

			if (color > white)
			{
				color = lightWhite;
			}
			else
			{
				color = color - color % colorStep;
			}

			return color;
		}

		private SizeF CalculateNewSize(Bitmap image, double maxWidth)
		{
			var imageWidth = image.Width;
			var cof = maxWidth / imageWidth;
			var newWidth = (int)maxWidth;
			var newHeight = (int)(cof * image.Height);

			return new SizeF() { Height = newWidth, Width = newHeight };
		}

		private Bitmap ResizeImage(Bitmap image, SizeF newSize)
		{
			Bitmap resizedimage;
			var resizeWidth = newSize.Width / (double)image.Width;
			var resizeHeight = newSize.Height / (double)image.Height;

			if (resizeWidth < resizeHeight)
			{
				var newWidth = (int)(resizeWidth * image.Width);
				var newHeigth = (int)(resizeWidth * image.Height);
				resizedimage = new Bitmap(image, new Size(newWidth, newHeigth));
			}
			else
			{
				var newWidth = (int)(resizeHeight * image.Width);
				var newHeigth = (int)(resizeHeight * image.Height);
				resizedimage = new Bitmap(image, new Size(newWidth, newHeigth));
			}

			return resizedimage;
		}

		private Bitmap Grayscale(Bitmap image)
		{
			var bitmap = new Bitmap(image);
			for (int idexWidth = 0; idexWidth < bitmap.Width; idexWidth++)
			{
				for (int indexHeight = 0; indexHeight < bitmap.Height; indexHeight++)
				{
					int grey = (int)(bitmap.GetPixel(idexWidth, indexHeight).R * 0.3
						+ bitmap.GetPixel(idexWidth, indexHeight).G * 0.59
						+ bitmap.GetPixel(idexWidth, indexHeight).B * 0.11);
					bitmap.SetPixel(idexWidth, indexHeight, Color.FromArgb(grey, grey, grey));
				}
			}
			image.Dispose();
			return bitmap;
		}

		private Dictionary<int, string> ToCellsColor(Dictionary<string, int> colors)
		{
			var result = new Dictionary<int, string>(colors.Count);
			foreach (var color in colors)
			{
				result.Add(color.Value, color.Key);
			}
			return result;
		}
	}
}
