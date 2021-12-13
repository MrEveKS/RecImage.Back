using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using ImageConverter.Models;

namespace ImageConverter
{
	public sealed class ImageConverter : IImageConverter, IDisposable
	{
		private bool _disposedValue;

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Convert image to point color object
		/// </summary>
		/// <param name="imageStream"> </param>
		/// <param name="options"> </param>
		/// <returns> </returns>
		public async Task<RecColor> ConvertToChars(Stream imageStream, ConvertOptions options)
		{
			var image = await ImageStreamConvert(imageStream, options);

			return ImageConvert(image, options);
		}

		private async Task<Bitmap> ImageStreamConvert(Stream imageStream, ConvertOptions options)
		{
			Bitmap image256;

			var image = (Bitmap) Image.FromStream(imageStream);
			var size = CalculateNewSize(image, options.Size);

			switch (options.ColorStep)
			{
				case ColorStep.Middle:
				case ColorStep.Big:
				case ColorStep.VeryBig:
				{
					image256 = ResizeImage(ConvertTo256(image), size);

					if (!options.Colored)
					{
						image256 = GrayScale(image256);
					}
				}

					break;
				default:
				{
					var imageResized = ResizeImage(image, size);

					if (!options.Colored)
					{
						imageResized = GrayScale(imageResized);
					}

					image256 = ConvertTo256(imageResized);
				}

					break;
			}

			await imageStream.DisposeAsync();

			return image256;
		}

		private RecColor ImageConvert(Bitmap image, ConvertOptions options)
		{
			var colorStep = (int) options.ColorStep;
			const int pixelSize = 1;
			const int pQ = pixelSize * pixelSize;

			var colors = new Dictionary<string, int>();

			var height = image.Height - image.Height % pixelSize;
			var width = image.Width - image.Width % pixelSize;

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
							var pixelColor = image.GetPixel(rowIndex, colIndex);
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
					} else
					{
						index = colors.Count + 1;
						colors.Add(webColor, index);
					}

					row.Add(index);
				}

				colCell.Add(row);
			}

			return new RecColor
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

		private Bitmap ConvertTo256(Bitmap image)
		{
			var result = image.Clone(new Rectangle(0, 0, image.Width, image.Height), PixelFormat.Format8bppIndexed);
			image.Dispose();

			return result;
		}

		private int UpdateColor(int color, int colorStep)
		{
			const int white = 248;
			const int lightWhite = 250;

			if (color > white)
			{
				color = lightWhite;
			} else
			{
				color -= color % colorStep;
			}

			return color;
		}

		private SizeF CalculateNewSize(Image image, double maxWidth)
		{
			var imageWidth = image.Width;
			var cof = maxWidth / imageWidth;
			var newWidth = (int) maxWidth;
			var newHeight = (int) (cof * image.Height);

			return new SizeF
				{ Height = newWidth, Width = newHeight };
		}

		private Bitmap ResizeImage(Image image, SizeF newSize)
		{
			Bitmap resizedImage;
			var resizeWidth = newSize.Width / (double) image.Width;
			var resizeHeight = newSize.Height / (double) image.Height;

			if (resizeWidth < resizeHeight)
			{
				var newWidth = (int) (resizeWidth * image.Width);
				var newHeight = (int) (resizeWidth * image.Height);
				resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
			} else
			{
				var newWidth = (int) (resizeHeight * image.Width);
				var newHeight = (int) (resizeHeight * image.Height);
				resizedImage = new Bitmap(image, new Size(newWidth, newHeight));
			}

			image.Dispose();

			return resizedImage;
		}

		private Bitmap GrayScale(Image image)
		{
			var bitmap = new Bitmap(image);

			for (var indexWidth = 0; indexWidth < bitmap.Width; indexWidth++)
			{
				for (var indexHeight = 0; indexHeight < bitmap.Height; indexHeight++)
				{
					var grey = (int) (bitmap.GetPixel(indexWidth, indexHeight).R * 0.3
									+ bitmap.GetPixel(indexWidth, indexHeight).G * 0.59
									+ bitmap.GetPixel(indexWidth, indexHeight).B * 0.11);

					bitmap.SetPixel(indexWidth, indexHeight, Color.FromArgb(grey, grey, grey));
				}
			}

			image.Dispose();

			return bitmap;
		}

		private Dictionary<int, string> ToCellsColor(Dictionary<string, int> colors)
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
			if (_disposedValue)
			{
				return;
			}

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
	}
}