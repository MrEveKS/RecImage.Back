using ImageConverter.Models;
using ImageToPuzzle.Infrastructure.Logging;
using ImageToPuzzle.Models;
using ImageToPuzzle.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ImageToPuzzle.Controllers
{
	[Route("api/[controller]/[action]")]
	[Produces("application/json")]
	public class GenerateController : Controller
	{
		private readonly IImageToPointService _imageConverter;
		private readonly IActionLogger _logger;

		public GenerateController(IImageToPointService imageConverter, IActionLogger logger)
		{
			_imageConverter = imageConverter;
			_logger = logger;
		}

		/// <summary>
		/// Convert image to points
		/// </summary>
		/// <param name="image"> image from Form </param>
		/// <param name="options"> image setting for converter options </param>
		/// <returns></returns>
		[HttpPost]
		public async Task<JsonResult> ConvertToPoints([FromForm] IFormFile image, [FromForm] ConvertOptions options)
		{
			try
			{
				_logger.InformationObject(options);
				Stopwatch stopwatch = Stopwatch.StartNew();
				var result = await _imageConverter.ConvertFromFile(image, options);
				_logger.Information("ConvertToPoints time", stopwatch.Elapsed.TotalMilliseconds);
				return new JsonResult(result);
			}
			catch (Exception ex)
			{
				_logger.ErrorObject(ex, image);
				throw;
			}
		}

		[HttpPost]
		public async Task<JsonResult> ConvertToPointsById([FromBody] ConvertFromNameOptions options)
		{
			try
			{
				_logger.InformationObject(options);
				Stopwatch stopwatch = Stopwatch.StartNew();
				var result = await _imageConverter.ConvertFromFileName(options);
				_logger.Information("ConvertToPointsByFileName time", stopwatch.Elapsed.TotalMilliseconds);
				return new JsonResult(result);
			}
			catch (Exception ex)
			{
				_logger.ErrorObject(ex, options);
				throw;
			}
		}
	}
}
