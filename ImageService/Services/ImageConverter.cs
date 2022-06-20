﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using ImageService.Models;

namespace ImageService.Services;

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
		var colorStep = (int) options.ColorStep;
		const int pixelSize = 1;
		const int separatePointSize = pixelSize * pixelSize;

		var colors = new Dictionary<string, int>();

		var image = (Bitmap) Image.FromStream(imageStream);
		var size = CalculateNewSize(image, options.Size);
		var imageResized = ResizeImage(image, size);

		if (!options.Colored)
		{
			imageResized = GrayScale(imageResized);
		}

		var heightWithSeparate = imageResized.Height - imageResized.Height % pixelSize;
		var widthHeightWithSeparate = imageResized.Width - imageResized.Width % pixelSize;

		var cells = new List<List<int>>(heightWithSeparate);

		for (var colIndex = 0; colIndex < heightWithSeparate; colIndex += pixelSize)
		{
			var row = new List<int>(widthHeightWithSeparate);

			for (var rowIndex = 0; rowIndex < widthHeightWithSeparate; rowIndex += pixelSize)
			{
				var red = 0;
				var green = 0;
				var blue = 0;

				for (var rIndex = rowIndex; rIndex < rowIndex + pixelSize; rIndex++)
				{
					for (var cIndex = colIndex; cIndex < colIndex + pixelSize; cIndex++)
					{
						var pixelColor = imageResized.GetPixel(rowIndex, colIndex);
						red += pixelColor.R;
						green += pixelColor.G;
						blue += pixelColor.B;
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
				} else
				{
					index = colors.Count + 1;
					colors.Add(webColor, index);
				}

				row.Add(index);
			}

			cells.Add(row);
		}

		await imageStream.DisposeAsync();

		return new ColorPoints
		{
			Cells = cells,
			CellsColor = ToCellsColor(colors)
		};
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
		} else
		{
			color -= color % colorStep;
		}

		return color;
	}

	private static SizeF CalculateNewSize(Image image, double maxWidth)
	{
		var imageWidth = image.Width;
		var cof = maxWidth / imageWidth;
		var newWidth = (int) maxWidth;
		var newHeight = (int) (cof * image.Height);

		return new SizeF
		{
			Height = newWidth,
			Width = newHeight
		};
	}

	private static Bitmap ResizeImage(Image image, SizeF newSize)
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

	private static Bitmap GrayScale(Image image)
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