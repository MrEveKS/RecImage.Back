using ImageConverter.Models;
using System.IO;

namespace ImageConverter
{
	public interface IImageConverter
	{
		RecColor ConvertToChars(Stream imageStream, ConvertOptions options);
	}
}