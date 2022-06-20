using System.Threading.Tasks;
using ImageService.Models;
using ImageToPuzzle.Models;
using Microsoft.AspNetCore.Http;

namespace ImageToPuzzle.Services;

public interface IImageToPointService
{
	Task<ColorPoints> ConvertFromFileName(ConvertFromNameOptions options);

	Task<ColorPoints> ConvertFromFile(IFormFile image, ConvertOptions options);
}