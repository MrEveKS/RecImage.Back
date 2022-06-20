using System.IO;
using System.Threading.Tasks;
using ImageService.Models;

namespace ImageService.Services;

public interface IImageConverter
{
	Task<ColorPoints> ConvertToColorPoints(Stream imageStream, ConvertOptions options);
}