using ImageConverter.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace ImageConverter
{
	public sealed class ImageConverter : IImageConverter, IDisposable
	{
		private bool _disposedValue;

		/// <summary>
		/// Convert image to point color object
		/// </summary>
		/// <param name="imageStream"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public async Task<RecColor> ConvertToChars(Stream imageStream, ConvertOptions options)
		{
			var colorStep = (int)options.ColorStep;
			const int pixelSize = 1;
			const int pQ = pixelSize * pixelSize;

			var colors = new Dictionary<string, int>();

			var image = ((Bitmap)Image.FromStream(imageStream));
			var size = CalculateNewSize(image, options.Size);
			var imageResized = ResizeImage(image, size);
			if (!options.Colored)
			{
				imageResized = Grayscale(imageResized);
			}

			var height = imageResized.Height - imageResized.Height % pixelSize;
			var width = imageResized.Width - imageResized.Width % pixelSize;

			var colCell = new List<List<int>>(height);
			for (var indexHeight = 0; indexHeight < height; indexHeight += pixelSize)
			{
				var row = new List<int>(width);
				for (var indexWidth = 0; indexWidth < width; indexWidth += pixelSize)
				{
					var r = 0;
					var g = 0;
					var b = 0;
					var rowIndex = indexWidth;
					var colIndex = indexHeight;

					for (var i = rowIndex; i < rowIndex + pixelSize; i++)
					{
						for (var j = colIndex; j < colIndex + pixelSize; j++)
						{
							var pixelColor = imageResized.GetPixel(rowIndex, colIndex);
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
			await imageStream.DisposeAsync();
			return new RecColor()
			{
				Cells = colCell,
				CellsColor = ToCellsColor(colors)
			};
		}

		private static Color GetNewColor(int nR, int nG, int nB, int colorStep)
		{
			nR = UpdateColor(nR, colorStep);
			nG = UpdateColor(nG, colorStep);
			nB = UpdateColor(nB, colorStep);

			return Color.FromArgb(nR, nG, nB);
		}

		private static int UpdateColor(int color, int colorStep)
		{
			const int white = 248;
			const int lightWhite = 250;

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

		private static SizeF CalculateNewSize(Image image, double maxWidth)
		{
			var imageWidth = image.Width;
			var cof = maxWidth / imageWidth;
			var newWidth = (int)maxWidth;
			var newHeight = (int)(cof * image.Height);

			return new SizeF() { Height = newWidth, Width = newHeight };
		}

		private static Bitmap ResizeImage(Image image, SizeF newSize)
		{
			Bitmap resizedImage;
			var resizeWidth = newSize.Width / (double)image.Width;
			var resizeHeight = newSize.Height / (double)image.Height;

			if (resizeWidth < resizeHeight)
			{
				var newWidth = (int)(resizeWidth * image.Width);
				var newHeight = (int)(resizeWidth * image.Height);
				resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
			}
			else
			{
				var newWidth = (int)(resizeHeight * image.Width);
				var newHeight = (int)(resizeHeight * image.Height);
				resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
			}
			image.Dispose();
			return resizedImage;
		}

		private static Bitmap Grayscale(Image image)
		{
			var bitmap = new Bitmap(image);
			for (var indexWidth = 0; indexWidth < bitmap.Width; indexWidth++)
			{
				for (var indexHeight = 0; indexHeight < bitmap.Height; indexHeight++)
				{
					var grey = (int)(bitmap.GetPixel(indexWidth, indexHeight).R * 0.3
					                 + bitmap.GetPixel(indexWidth, indexHeight).G * 0.59
					                 + bitmap.GetPixel(indexWidth, indexHeight).B * 0.11);
					bitmap.SetPixel(indexWidth, indexHeight, Color.FromArgb(grey, grey, grey));
				}
			}
			image.Dispose();
			return bitmap;
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

		private void Dispose(bool disposing)
		{
			if (_disposedValue) return;
			if (disposing)
			{
				GC.Collect(GC.MaxGeneration);
			}

			_disposedValue = true;
		}

		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		
		~ImageConverter()
		{
			Dispose (false);
		}
	}
}
