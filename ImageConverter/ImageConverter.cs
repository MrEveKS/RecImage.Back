using ImageConverter.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ImageConverter
{
	public class ImageConverter : IImageConverter
	{
		private const double MAX_WIDTH = 250d;

		private readonly GenerateFontWeightsNum _generateFontWeightsNum;

		public ImageConverter()
		{
			_generateFontWeightsNum = new GenerateFontWeightsNum();
		}

		public List<List<ColoredChar>> ConvertToChars(Stream imageStream)
		{
			return ConvertToChars(imageStream);
		}

		public List<List<ColoredChar>> ConvertToChars(Stream imageStream, int maxWidth)
		{
			return ConvertToChars(imageStream, (double)maxWidth);
		}

		private List<List<ColoredChar>> ConvertToChars(Stream imageStream, double? maxWidth = null)
		{
			List<List<ColoredChar>> coloredText;
			using (var image = (Bitmap)Bitmap.FromStream(imageStream))
			{
				var newSize = CalculateNewSize(image, maxWidth);
				using (var resizedImage = ResizeImage(image, newSize))
				using (var greyImage = Grayscale(resizedImage))
				{
					coloredText = ConvertToASCII(greyImage);
				}
			}
			_generateFontWeightsNum.Dispose();
			return coloredText;
		}

		private unsafe List<List<ColoredChar>> ConvertToASCII(Bitmap bwImage)
		{
			var width = 1;
			var height = 1;

			var characters = _generateFontWeightsNum.Generate();
			var coloredText = new List<List<ColoredChar>>(bwImage.Height);

			int charWidth = characters[0].Image.Width;
			int charHeight = characters[0].Image.Height;

			using (var clone = (Bitmap)bwImage.Clone())
			{
				var bitmapdata = clone.LockBits(new Rectangle(0, 0, clone.Width, clone.Height),
					System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				var pixelSize = 4;


				for (int j = 0; j < bwImage.Height; j += height)
				{
					var ct = new List<ColoredChar>(bwImage.Height);
					byte* destPixels = (byte*)bitmapdata.Scan0 + (j * bitmapdata.Stride);

					for (int i = 0; i < bwImage.Width; i += width)
					{
						var b = (int)destPixels[i * pixelSize];
						var g = (int)destPixels[i * pixelSize + 1];
						var r = (int)destPixels[i * pixelSize + 2];
						var color = Color.FromArgb(r, g, b);

						double targetvalue = 0;

						for (int nj = j; nj < j + height; nj++)
						{
							for (int ni = i; ni < i + width; ni++)
							{
								if (bwImage.Width > ni && bwImage.Height > nj)
								{
									targetvalue += bwImage.GetPixel(ni, nj).R;
								}
								else
								{
									targetvalue += 128;
								}

							}
						}
						targetvalue /= (width * height);
						var closestchar = GetClosestchar(characters, targetvalue);

						ct.Add(new ColoredChar
						{
							Character = closestchar.Character,
							WebColor = ColorTranslator.ToHtml(color)
						});
					}
					coloredText.Add(ct);
				}
			}

			return coloredText;
		}

		private SizeF CalculateNewSize(Bitmap image, double? maxWidth = null)
		{
			var imageWidth = image.Width;
			var cof = (maxWidth ?? MAX_WIDTH) / imageWidth;
			var newWidth = (int)(maxWidth ?? MAX_WIDTH);
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

		private WeightedChar GetClosestchar(List<WeightedChar> characters, double targetvalue)
		{
			var minWeight = -1d;
			for (int index = 0; index < characters.Count; index++)
			{
				var character = characters[index];
				var curMin = Math.Abs(character.Weight - targetvalue);
				if (minWeight == -1 || curMin < minWeight)
				{
					minWeight = curMin;
				}
			}

			WeightedChar result = null;

			for (int index = 0; index < characters.Count; index++)
			{
				var character = characters[index];
				if (Math.Abs(character.Weight - targetvalue) == minWeight)
				{
					result = character;
					break;
				}
			}

			return result;
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
			return bitmap;
		}
	}
}
