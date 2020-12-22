using ImageConverter;
using ImageConverter.Models;
using ImageToPuzzle.Infrastructure.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ImageToPuzzle.Controllers
{
	[Route("api/[controller]/[action]")]
	public class GenerateController : Controller
	{
		private readonly IImageConverter _imageConverter;
		private readonly IActionLoger _loger;

		public GenerateController(IImageConverter imageConverter, IActionLoger loger)
		{
			_imageConverter = imageConverter;
			_loger = loger;
		}

		[HttpPost]
		public async Task<RecColor> ConvertToChar([FromForm] IFormFile image, [FromForm] ConvertOptions options)
		{
			try
			{
				_loger.InformationObject(options);
				Stopwatch stopwatch = Stopwatch.StartNew();
				using var memoryStream = new MemoryStream();
				await image.CopyToAsync(memoryStream);
				var result = await _imageConverter.ConvertToChars(memoryStream, options);
				_loger.Information("time", stopwatch.Elapsed.TotalSeconds);
				return result;
			}
			catch (Exception ex)
			{
				_loger.ErrorObject(ex, image);
				throw;
			}

		}
	}
}
