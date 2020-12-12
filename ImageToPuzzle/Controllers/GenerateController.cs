using ImageConverter;
using ImageConverter.Models;
using ImageToPuzzle.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ImageToPuzzle.Controllers
{
	[Route("api/[controller]/[action]")]
	public class GenerateController : Controller
	{
		private readonly IImageConverter _imageConverter;

		public GenerateController(IImageConverter imageConverter)
		{
			_imageConverter = imageConverter;
		}

		[HttpPost]
		public async Task<RecColor> ConvertToChar([FromForm] IFormFile image, [FromForm] ConvertOptions options)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			using var memoryStream = new MemoryStream();
			await image.CopyToAsync(memoryStream);
			var result = _imageConverter.ConvertToChars(memoryStream, options);
			System.Console.WriteLine($"time: {stopwatch.Elapsed.TotalSeconds}");
			return result;
		}
	}
}
