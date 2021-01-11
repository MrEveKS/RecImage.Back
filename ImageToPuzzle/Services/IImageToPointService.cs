using ImageConverter.Models;
using ImageToPuzzle.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ImageToPuzzle.Services
{
	public interface IImageToPointService
	{
		Task<RecColor> ConvertFromFileName(ConvertFromNameOptions options);
		Task<RecColor> ConvertFromFile(IFormFile image, ConvertOptions options);
	}
}