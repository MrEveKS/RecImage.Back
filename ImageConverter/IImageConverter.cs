using ImageConverter.Models;
using System.Collections.Generic;
using System.IO;

namespace ImageConverter
{
	public interface IImageConverter
	{
		List<List<ColoredChar>> ConvertToChars(Stream imageStream);
		List<List<ColoredChar>> ConvertToChars(Stream imageStream, int maxWidth);
	}
}