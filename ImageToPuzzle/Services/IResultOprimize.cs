using ImageConverter.Models;
using ImageToPuzzle.Models;
using System.Collections.Generic;

namespace ImageToPuzzle.Services
{
	public interface IResultOprimize
	{
		RecColor Convert(List<List<ColoredChar>> coloredChars);
	}
}