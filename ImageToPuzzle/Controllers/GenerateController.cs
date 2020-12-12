using ImageConverter;
using ImageConverter.Models;
using ImageToPuzzle.Models;
using ImageToPuzzle.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ImageToPuzzle.Controllers
{
	[Route("api/[controller]/[action]")]
	public class GenerateController : Controller
	{
		private readonly IImageConverter _imageConverter;
		private readonly IResultOprimize _resultOprimize;

		public GenerateController(IImageConverter imageConverter, IResultOprimize resultOprimize)
		{
			_imageConverter = imageConverter;
			_resultOprimize = resultOprimize;
		}

		[HttpPost]
		public async Task<RecColor> ConvertToChar([FromForm] IFormFile image, [FromForm] int size = 250)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			using var memoryStream = new MemoryStream();
			await image.CopyToAsync(memoryStream);
			var result = _imageConverter.ConvertToChars(memoryStream, size);
			System.Console.WriteLine($"time: {stopwatch.Elapsed.TotalSeconds}");
			return _resultOprimize.Convert(result);
		}
	}
}
