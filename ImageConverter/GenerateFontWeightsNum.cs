using ImageConverter.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ImageConverter
{
	internal class GenerateFontWeightsNum : IDisposable
	{
		private readonly List<string> _numbers;
		private readonly Dictionary<string, CharImage> _numbersWeight;

		public GenerateFontWeightsNum()
		{
			_numbers = GenerateRange(0, 512);
			_numbersWeight = GenerateWeights();
		}

		public List<WeightedChar> Generate()
		{
			var weightedChars = new List<WeightedChar>(_numbers.Count);
			for (int index = 0; index < _numbers.Count; index++)
			{
				var number = _numbers[index];
				var forweighting = new WeightedChar()
				{
					Weight = _numbersWeight[number].Weight,
					Character = number,
					Image = _numbersWeight[number].Image
				};
				weightedChars.Add(forweighting);
			}

			weightedChars = LinearMap(weightedChars);

			return weightedChars;
		}

		private Dictionary<string, CharImage> GenerateWeights()
		{
			var commonsize = GetGeneralSizeNum();
			var result = new Dictionary<string, CharImage>();
			for (int index = 0; index < _numbers.Count; index++)
			{
				var number = _numbers[index];
				var charImage = DrawText(number, Color.Black, Color.White, commonsize);
				var weight = GetWeight(charImage, commonsize);
				result.Add(number, new CharImage()
				{
					Image = charImage,
					Weight = weight
				});
			}

			return result;
		}

		private List<string> GenerateRange(int from, int to)
		{
			var result = new List<string>(to - from);
			for (int index = 0; index < to; index++)
			{
				result.Add(index.ToString());
			}

			return result;
		}

		private double GetWeight(Image charImage, SizeF size)
		{
			Bitmap btm = new Bitmap(charImage);
			double totalsum = 0;

			for (int i = 0; i < btm.Width; i++)
			{
				for (int j = 0; j < btm.Height; j++)
				{
					totalsum = totalsum + (btm.GetPixel(i, j).R
										+ btm.GetPixel(i, j).G
										+ btm.GetPixel(i, j).B) / 3;
				}
			}
			return totalsum / (size.Height * size.Width);
		}

		private SizeF GetGeneralSizeNum()
		{
			SizeF generalsize = new SizeF(0, 0);
			for (int index = 0; index < _numbers.Count; index++)
			{
				var number = _numbers[index];
				using (var img = new Bitmap(1, 1))
				using (var drawing = Graphics.FromImage(img))
				{
					var textSize = drawing.MeasureString(number, SystemFonts.DefaultFont);
					if (textSize.Width > generalsize.Width)
					{
						generalsize.Width = textSize.Width;
					}
					if (textSize.Height > generalsize.Height)
					{
						generalsize.Height = textSize.Height;
					}
				}
			}

			generalsize.Width = ((int)generalsize.Width);
			generalsize.Height = ((int)generalsize.Height);

			if (generalsize.Width > generalsize.Height)
			{
				generalsize.Height = generalsize.Width;
			}
			else
			{
				generalsize.Width = generalsize.Height;
			}

			return generalsize;
		}

		private List<WeightedChar> LinearMap(List<WeightedChar> characters)
		{
			var max = GetMaxWeight(characters);
			var min = GetMinWeight(characters);
			var range = 255d;
			var slope = range / (max - min);
			var n = -min * slope;
			foreach (var character in characters)
			{
				character.Weight = slope * character.Weight + n;
			}
			return characters;
		}

		private Image DrawText(string text, Color textColor, Color backColor, SizeF WidthAndHeight)
		{
			var dummyImg = new Bitmap(1, 1);
			var dummyDrawing = Graphics.FromImage(dummyImg);
			var textSize = dummyDrawing.MeasureString(text, SystemFonts.DefaultFont);
			dummyImg.Dispose();
			dummyDrawing.Dispose();

			Image img = new Bitmap(1, 1);
			Graphics drawing = Graphics.FromImage(img);

			img.Dispose();
			drawing.Dispose();

			img = new Bitmap((int)WidthAndHeight.Width, (int)WidthAndHeight.Height);
			drawing = Graphics.FromImage(img);

			drawing.Clear(backColor);

			Brush textBrush = new SolidBrush(textColor);
			drawing.DrawString(text, SystemFonts.DefaultFont, textBrush, (WidthAndHeight.Width - textSize.Width) / 2, 0);
			drawing.Save();

			textBrush.Dispose();
			drawing.Dispose();

			return img;
		}

		private double GetMinWeight(List<WeightedChar> characters)
		{
			var min = characters[0].Weight;
			for (int index = 1; index < characters.Count; index++)
			{
				var weightedChar = characters[index];
				if (weightedChar.Weight < min)
				{
					min = weightedChar.Weight;
				}
			}
			return min;
		}

		private double GetMaxWeight(List<WeightedChar> characters)
		{
			var max = characters[0].Weight;
			for (int index = 1; index < characters.Count; index++)
			{
				var weightedChar = characters[index];
				if (weightedChar.Weight > max)
				{
					max = weightedChar.Weight;
				}
			}
			return max;
		}

		public void Dispose()
		{
			if (_numbersWeight != null)
			{
				foreach (var item in _numbersWeight)
				{
					item.Value.Image.Dispose();
				}
			}
		}
	}
}
