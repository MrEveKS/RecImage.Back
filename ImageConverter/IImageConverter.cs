using ImageConverter.Models;
using System.IO;
using System.Threading.Tasks;

namespace ImageConverter
{
	public interface IImageConverter
	{
		Task<RecColor> ConvertToChars(Stream imageStream, ConvertOptions options);
	}
}